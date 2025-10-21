using UnityEngine;
using System.Collections;

public class FloorCleaning : MonoBehaviour
{
    [Header("Cleaning Settings")]
    public float cleanTimePerPiece = 1f;   // How long to clean each dirt piece
    public GameObject[] dirtPieces;        // Assign each squished cylinder here in the inspector

    [HideInInspector] public bool isPlayerNearby;
    private float holdTime;
    private int currentPieceIndex = 0;

    private PlayerControls controls;

    // Event to notify MessManager when this mess is fully cleaned
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

        if (controls.Gameplay.Interact.IsPressed())
        {
            holdTime += Time.deltaTime;

            if (holdTime >= cleanTimePerPiece)
            {
                // Remove the current dirt piece
                Destroy(dirtPieces[currentPieceIndex]);
                SoundEffectManager.Play("MopSound");
                currentPieceIndex++;
                holdTime = 0f;

                // Notify the manager if this mess is fully cleaned
                if (currentPieceIndex >= dirtPieces.Length)
                {
                    OnMessCleaned?.Invoke(gameObject);
                }
            }
        }
        else if (controls.Gameplay.Interact.WasReleasedThisFrame())
        {
            holdTime = 0f; // Reset progress if player stops
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            isPlayerNearby = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            holdTime = 0f;
        }
    }
}
