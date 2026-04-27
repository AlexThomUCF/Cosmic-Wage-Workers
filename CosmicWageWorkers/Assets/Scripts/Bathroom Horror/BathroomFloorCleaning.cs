using UnityEngine;

public class BathroomFloorCleaning : MonoBehaviour
{
    public float cleanTimePerPiece = 1f;
    public GameObject[] dirtPieces;

    private int currentPieceIndex = 0;
    private float holdTime;
    private bool isPlayerNearby;

    private PickupMop playerMop;
    private PlayerControls controls;

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
        if (!isPlayerNearby) return;
        if (currentPieceIndex >= dirtPieces.Length) return;
        if (playerMop == null || !playerMop.IsHoldingMop()) return;

        if (controls.Gameplay.Use.IsPressed())
        {
            holdTime += Time.deltaTime;

            float moveDistance = Mathf.Sin(Time.time * 5f) * 0.5f;
            playerMop.cleaningOffset = new Vector3(moveDistance, 0f, 0f);

            if (holdTime >= cleanTimePerPiece)
            {
                Destroy(dirtPieces[currentPieceIndex]);
                currentPieceIndex++;
                holdTime = 0f;
            }
        }
        else
        {
            holdTime = 0f;

            if (playerMop != null)
                playerMop.cleaningOffset = Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerMop = other.GetComponent<PickupMop>();
            isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;

            if (playerMop != null)
                playerMop.cleaningOffset = Vector3.zero;

            playerMop = null;
            holdTime = 0f;
        }
    }
}
