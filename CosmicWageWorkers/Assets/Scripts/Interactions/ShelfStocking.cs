using UnityEngine;

public class ShelfStocking : MonoBehaviour
{
    [Header("Stocking Settings")]
    public GameObject cubePrefab;
    public int cubesPerRow = 5;
    public float spacing = 0.4f;
    public float rowSpacing = 0.4f;
    public Vector3 cubeRotationOffset = Vector3.zero;

    [Header("Start Points (One per vertical shelf)")]
    public Transform[] startPoints;

    [Header("Zone Highlight")]
    public Renderer[] zoneRenderers;
    public Color highlightColor = Color.yellow;
    private Color[] originalColors;

    [Header("Zone Info")]
    public BoxManager boxManager;
    public int zoneIndex;

    [Header("Input Settings")]
    public float rowCooldown = 0.3f;
    private float lastRowTime = 0f;

    private PlayerControls controls;
    private BoxPickUp playerPickup;
    private bool isPlayerNearby;

    private int nextShelfIndex = 0;
    private int rowInShelf = 0;
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
        originalColors = new Color[zoneRenderers.Length];
        for (int i = 0; i < zoneRenderers.Length; i++)
        {
            if (zoneRenderers[i] != null)
                originalColors[i] = zoneRenderers[i].material.color;
        }

        nextShelfIndex = ShelfProgressData.GetNextShelf(zoneIndex);
        rowInShelf = ShelfProgressData.GetRowInShelf(zoneIndex);

        if (boxManager != null)
            boxManager.SetRowsStockedForZone(zoneIndex, nextShelfIndex * rowsPerShelf + rowInShelf);

        for (int shelf = 0; shelf < nextShelfIndex; shelf++)
            SpawnFullShelf(shelf);
        if (rowInShelf > 0 && nextShelfIndex < startPoints.Length)
            SpawnPartialShelf(nextShelfIndex, rowInShelf);
    }

    private void Update()
    {
        UpdateZoneHighlight();

        if (!isPlayerNearby || playerPickup == null || !playerPickup.IsHoldingBox())
            return;

        if (boxManager.GetCurrentZoneIndex() != zoneIndex) return;
        if (nextShelfIndex >= startPoints.Length) return;

        if (controls.Gameplay.Use.IsPressed() && Time.time - lastRowTime >= rowCooldown)
        {
            StockRow();
            lastRowTime = Time.time;
        }
    }

    private void StockRow()
    {
        Transform currentShelfStart = startPoints[nextShelfIndex];

        for (int i = 0; i < cubesPerRow; i++)
        {
            Vector3 pos = currentShelfStart.position
                          + currentShelfStart.right * (i * spacing)
                          + currentShelfStart.forward * (rowInShelf * rowSpacing);

            Quaternion rot = currentShelfStart.rotation * Quaternion.Euler(cubeRotationOffset);
            Instantiate(cubePrefab, pos, rot, transform);
        }

        SoundEffectManager.Play("StockSound");

        rowInShelf++;
        if (rowInShelf >= rowsPerShelf)
        {
            rowInShelf = 0;
            nextShelfIndex++;
        }

        boxManager?.OnRowStocked();
        ShelfProgressData.SetShelfProgress(zoneIndex, nextShelfIndex, rowInShelf);
    }

    private void UpdateZoneHighlight()
    {
        bool shouldHighlight = boxManager.GetCurrentZoneIndex() == zoneIndex;

        for (int i = 0; i < zoneRenderers.Length; i++)
        {
            if (zoneRenderers[i] != null)
            {
                if (shouldHighlight)
                {
                    float intensity = ((Mathf.Sin(Time.time * 2f) + 1f) / 2f) * 0.5f;
                    zoneRenderers[i].material.color = Color.Lerp(originalColors[i], highlightColor, intensity);
                }
                else
                    zoneRenderers[i].material.color = originalColors[i];
            }
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

    private void SpawnFullShelf(int shelfIndex)
    {
        Transform start = startPoints[shelfIndex];
        for (int row = 0; row < rowsPerShelf; row++)
        {
            for (int i = 0; i < cubesPerRow; i++)
            {
                Vector3 pos = start.position
                              + start.right * (i * spacing)
                              + start.forward * (row * rowSpacing);

                Quaternion rot = start.rotation * Quaternion.Euler(cubeRotationOffset);
                Instantiate(cubePrefab, pos, rot, transform);
            }
        }
    }

    private void SpawnPartialShelf(int shelfIndex, int rows)
    {
        Transform start = startPoints[shelfIndex];
        for (int row = 0; row < rows; row++)
        {
            for (int i = 0; i < cubesPerRow; i++)
            {
                Vector3 pos = start.position
                              + start.right * (i * spacing)
                              + start.forward * (row * rowSpacing);

                Quaternion rot = start.rotation * Quaternion.Euler(cubeRotationOffset);
                Instantiate(cubePrefab, pos, rot, transform);
            }
        }
    }
}