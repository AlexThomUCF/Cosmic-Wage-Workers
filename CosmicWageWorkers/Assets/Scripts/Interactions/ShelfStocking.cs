using UnityEngine;

public class ShelfStocking : MonoBehaviour
{
    [Header("Stocking Settings")]
    public GameObject cubePrefab;
    public int cubesPerRow = 5;
    public float spacing = 0.4f;      // Distance along shelf depth (Z)
    public float rowSpacing = 0.4f;   // Distance between rows (Y axis)

    [Header("Start Points (One per vertical shelf)")]
    public Transform[] startPoints;   // 4 transforms for top → bottom shelves

    [Header("Zone Info")]
    public BoxManager boxManager;
    public int zoneIndex;   // Stock zone index this shelf belongs to

    private PlayerControls controls;
    private BoxPickUp playerPickup;
    private bool isPlayerNearby;

    private int nextShelfIndex = 0; // Which shelf we're currently stocking
    private int rowInShelf = 0;     // Row within current shelf
    private const int rowsPerShelf = 2;

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
        if (!isPlayerNearby || playerPickup == null || !playerPickup.IsHoldingBox())
            return;

        // Only stock if this shelf belongs to the CURRENT zone
        if (boxManager.GetCurrentZoneIndex() != zoneIndex) return;

        // Only stock if there are shelves left
        if (nextShelfIndex >= startPoints.Length) return;

        // Stock row when Use button pressed
        if (controls.Gameplay.Use.IsPressed())
            StockRow();
    }

    private void StockRow()
    {
        Transform currentShelfStart = startPoints[nextShelfIndex];

        for (int i = 0; i < cubesPerRow; i++)
        {
            Vector3 pos = currentShelfStart.position
                          + currentShelfStart.right * (i * spacing)       // cubes side by side
                          + currentShelfStart.forward * (rowInShelf * rowSpacing); // rows back along shelf

            Instantiate(cubePrefab, pos, Quaternion.identity, transform);
        }

        SoundEffectManager.Play("StockSound");

        rowInShelf++;

        if (rowInShelf >= rowsPerShelf)
        {
            rowInShelf = 0;
            nextShelfIndex++;
        }

        boxManager.OnRowStocked();

        playerPickup.ForceDropBox();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerPickup = other.GetComponent<BoxPickUp>();
            isPlayerNearby = playerPickup != null;
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