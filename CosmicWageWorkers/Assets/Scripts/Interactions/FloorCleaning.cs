using UnityEngine;
using System.Collections;

public class FloorCleaning : MonoBehaviour
{
    [Header("Cleaning Settings")]
    public float cleanTimePerPiece = 1f;   // How long to clean each dirt piece
    public GameObject[] dirtPieces;        // Assign each squished cylinder here in the inspector

    private bool isPlayerNearby;
    private bool isCleaning;
    private float holdTime;
    private int currentPieceIndex = 0;

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
        if (!isPlayerNearby || isCleaning || currentPieceIndex >= dirtPieces.Length) return;

        if (controls.Gameplay.Interact.IsPressed())
        {
            holdTime += Time.deltaTime;

            if (holdTime >= cleanTimePerPiece)
            {
                // Remove the current dirt piece
                Destroy(dirtPieces[currentPieceIndex]);
                currentPieceIndex++;
                holdTime = 0f;
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