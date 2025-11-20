using UnityEngine;

public class ShelfStocking : MonoBehaviour
{
    [Header("Stocking Settings")]
    public GameObject cubePrefab;
    public int cubesPerRow = 5;
    public float spacing = 0.4f;      // distance between cubes in a row (X axis)
    public float rowSpacing = 0.4f;   // distance between rows along Z

    [Header("Start Points (One per vertical shelf)")]
    public Transform[] startPoints;   // 4 shelves top → bottom

    [Header("Zone Highlight")]
    public Renderer[] zoneRenderers;  // all renderers in this stock zone
    public Color highlightColor = Color.yellow;
    private Color[] originalColors;

    [Header("Zone Info")]
    public BoxManager boxManager;
    public int zoneIndex;   // Which stock zone this shelf belongs to

    [Header("Input Settings")]
    public float rowCooldown = 0.3f; // seconds between rows
    private float lastRowTime = 0f;

    private PlayerControls controls;
    private BoxPickUp playerPickup;
    private bool isPlayerNearby;

    private int nextShelfIndex = 0; // current active shelf
    private int rowInShelf = 0;     // rows stocked in current shelf
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

    private void Start()
    {
        // store original colors
        originalColors = new Color[zoneRenderers.Length];
        for (int i = 0; i < zoneRenderers.Length; i++)
        {
            if (zoneRenderers[i] != null)
                originalColors[i] = zoneRenderers[i].material.color;
        }
    }

    private void Update()
    {
        UpdateZoneHighlight();

        if (!isPlayerNearby || playerPickup == null || !playerPickup.IsHoldingBox())
            return;

        // Only stock if this shelf belongs to the current zone
        if (boxManager.GetCurrentZoneIndex() != zoneIndex) return;

        if (nextShelfIndex >= startPoints.Length) return;

        // Only allow stocking if cooldown has passed
        if (controls.Gameplay.Use.IsPressed() && Time.time - lastRowTime >= rowCooldown)
        {
            StockRow();
            lastRowTime = Time.time; // reset cooldown
        }
    }

    private void StockRow()
    {
        Transform currentShelfStart = startPoints[nextShelfIndex];

        for (int i = 0; i < cubesPerRow; i++)
        {
            Vector3 pos = currentShelfStart.position
                          + currentShelfStart.right * (i * spacing)       // cubes side by side (X)
                          + currentShelfStart.forward * (rowInShelf * rowSpacing); // rows along Z

            Instantiate(cubePrefab, pos, Quaternion.identity, transform);
        }

        SoundEffectManager.Play("StockSound");

        rowInShelf++;

        if (rowInShelf >= rowsPerShelf)
        {
            rowInShelf = 0;
            nextShelfIndex++;
        }

        // Notify BoxManager
        boxManager?.OnRowStocked();
    }

    private void UpdateZoneHighlight()
    {
        bool shouldHighlight = boxManager.GetCurrentZoneIndex() == zoneIndex;

        for (int i = 0; i < zoneRenderers.Length; i++)
        {
            if (zoneRenderers[i] != null)
                zoneRenderers[i].material.color = shouldHighlight ? highlightColor : originalColors[i];
        }
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