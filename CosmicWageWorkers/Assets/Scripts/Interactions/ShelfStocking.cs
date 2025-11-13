using UnityEngine;

public class ShelfStocking : MonoBehaviour
{
    [Header("Stocking Settings")]
    public GameObject cubePrefab;
    public int cubesPerRow = 5;
    public float spacing = 0.4f;      // Distance between items in a row (Z axis)
    public float rowSpacing = 0.4f;   // Distance between front/back row (X axis)
    public Transform startPoint;

    [Header("Shelf Info")]
    public int shelfIndex; // Optional label
    public BoxManager boxManager;

    private PlayerControls controls;
    private BoxPickUp playerPickup;
    private bool isPlayerNearby;

    // Tracks how many rows have been stocked for this shelf
    private int nextRowIndex = 0;
    private int maxRows = 2;

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
        if (!isPlayerNearby || playerPickup == null || !playerPickup.IsHoldingBox()) return;

        // Only stock if shelf has rows left
        if (nextRowIndex >= maxRows) return;

        // Check if Use button pressed
        if (controls.Gameplay.Use.IsPressed())
        {
            StockRow();
        }
    }

    private void StockRow()
    {
        // Spawn a row of cubes
        for (int i = 0; i < cubesPerRow; i++)
        {
            Vector3 pos = startPoint.position
                          + transform.forward * (i * spacing)           // row extends along Z axis
                          + transform.right * (nextRowIndex * rowSpacing); // front/back row along X axis

            Instantiate(cubePrefab, pos, Quaternion.identity, transform);
        }

        SoundEffectManager.Play("StockSound");

        nextRowIndex++;

        // Notify BoxManager
        boxManager?.OnShelfStocked(shelfIndex);

        // Force drop the box after stocking the row
        playerPickup.ForceDropBox();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerPickup = other.GetComponent<BoxPickUp>();
            if (playerPickup != null)
                isPlayerNearby = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            playerPickup = null;
        }
    }
}
