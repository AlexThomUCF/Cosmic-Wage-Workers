using UnityEngine;

public class FloorCleaning : MonoBehaviour
{
    public float cleanTimePerPiece = 1f;
    public GameObject[] dirtPieces;

    private int currentPieceIndex = 0;
    private float holdTime;
    private bool isPlayerNearby;

    private PickupMop playerMop;
    private PlayerControls controls;
    private GameObject player;

    public event System.Action<GameObject> OnMessCleaned;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    private void Update()
    {
        if (!isPlayerNearby || currentPieceIndex >= dirtPieces.Length) return;
        if (playerMop == null || !playerMop.IsHoldingMop()) return;

        if (controls.Gameplay.Use.IsPressed())
        {
            holdTime += Time.deltaTime;

            float moveDistance = Mathf.Sin(Time.time * 5f) * 0.5f;
            playerMop.cleaningOffset = new Vector3(moveDistance, 0f, 0f);

            if (holdTime >= cleanTimePerPiece)
            {
                Destroy(dirtPieces[currentPieceIndex]);
                SoundEffectManager.Play("MopSound");

                currentPieceIndex++;
                holdTime = 0f;

                if (currentPieceIndex >= dirtPieces.Length)
                {
                    OnMessCleaned?.Invoke(gameObject);
                    playerMop.cleaningOffset = Vector3.zero;
                }
            }
        }
        else if (controls.Gameplay.Use.WasReleasedThisFrame())
        {
            holdTime = 0f;
            playerMop.cleaningOffset = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            playerMop = player.GetComponent<PickupMop>();
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            player = null;
            playerMop = null;
            holdTime = 0f;
        }
    }
}