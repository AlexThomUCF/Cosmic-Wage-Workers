using UnityEngine;

public class FloorCleaning : MonoBehaviour
{
    [Header("Cleaning Settings")]
    public float cleanTimePerPiece = 1f;
    public GameObject[] dirtPieces;

    [Header("Mop Swing Settings")]
    public float swingAngle = 30f;      // Maximum swing angle
    public float swingSpeed = 5f;       // Swing speed

    private int currentPieceIndex = 0;
    private float holdTime;
    private bool isPlayerNearby;

    private PickupMop playerMop;
    private PlayerControls controls;
    private GameObject player;

    private Transform mopTransform;
    private Quaternion initialRotation;

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

        // Ensure mopTransform and initialRotation are set
        if (mopTransform == null)
        {
            mopTransform = playerMop.handHoldPoint;
            initialRotation = mopTransform.localRotation;
        }

        if (controls.Gameplay.Use.IsPressed())
        {
            holdTime += Time.deltaTime;

            // Swing mop back and forth
            if (mopTransform != null)
            {
                float angle = Mathf.Sin(Time.time * swingSpeed) * swingAngle;
                mopTransform.localRotation = initialRotation * Quaternion.Euler(angle, 0f, 0f);
            }

            // Clean dirt pieces
            if (holdTime >= cleanTimePerPiece)
            {
                Destroy(dirtPieces[currentPieceIndex]);
                SoundEffectManager.Play("MopSound");

                currentPieceIndex++;
                holdTime = 0f;

                if (currentPieceIndex >= dirtPieces.Length)
                {
                    OnMessCleaned?.Invoke(gameObject);

                    // Snap mop upright
                    if (mopTransform != null)
                        mopTransform.localRotation = initialRotation;
                }
            }
        }
        else if (controls.Gameplay.Use.WasReleasedThisFrame())
        {
            holdTime = 0f;

            // Snap mop upright immediately
            if (mopTransform != null)
                mopTransform.localRotation = initialRotation;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            playerMop = player.GetComponent<PickupMop>();
            isPlayerNearby = true;

            if (playerMop != null && playerMop.IsHoldingMop())
            {
                mopTransform = playerMop.handHoldPoint;
                initialRotation = mopTransform.localRotation;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            player = null;
            playerMop = null;
            mopTransform = null;
            holdTime = 0f;
        }
    }
}