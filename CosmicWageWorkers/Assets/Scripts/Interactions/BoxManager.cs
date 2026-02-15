using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoxManager : MonoBehaviour
{
    [Header("Boxes & Zones")]
    public GameObject boxPrefab;
    public List<Transform> stockZones;
    public List<Transform> boxSpawnPoints; // predefined spawn points for each zone
    public float spawnHeight = 1f;
    public float spawnDelay = 1f;

    [Header("UI")]
    public TextMeshProUGUI percentText;

    private int currentZoneIndex = 0;
    private int rowsStockedThisZone = 0;
    private const int rowsPerZone = 8; // 4 shelves × 2 rows
    private GameObject currentBox;

    private void Start()
    {
        // Restore saved progress
        currentZoneIndex = 0;
        for (int z = 0; z < stockZones.Count; z++)
        {
            int stockedRows = ShelfProgressData.GetRowsStockedThisZone(z);
            if (stockedRows >= rowsPerZone)
                currentZoneIndex = Mathf.Max(currentZoneIndex, z + 1);
        }

        // Calculate rows stocked for the current zone
        if (currentZoneIndex < stockZones.Count)
            rowsStockedThisZone = ShelfProgressData.GetRowsStockedThisZone(currentZoneIndex);

        // Spawn box if zone incomplete
        if (currentZoneIndex < stockZones.Count)
            SpawnBox();
    }

    private void Update()
    {
        if (percentText != null)
            percentText.text = $"{currentZoneIndex}/{stockZones.Count} Zones Stocked";
    }

    private void SpawnBox()
    {
        if (currentZoneIndex >= stockZones.Count || boxSpawnPoints.Count == 0) return;

        Transform spawnPoint = boxSpawnPoints[Mathf.Min(currentZoneIndex, boxSpawnPoints.Count - 1)];
        Vector3 pos = spawnPoint.position + Vector3.up * spawnHeight;

        currentBox = Instantiate(boxPrefab, pos, spawnPoint.rotation);
        currentBox.tag = "Box";

        if (currentBox.GetComponent<Collider>() == null)
            currentBox.AddComponent<BoxCollider>();

        if (currentBox.GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = currentBox.AddComponent<Rigidbody>();
            rb.isKinematic = false;
        }
    }

    public void OnRowStocked()
    {
        rowsStockedThisZone++;
        int nextShelf = rowsStockedThisZone / 2;
        int rowInShelf = rowsStockedThisZone % 2;

        ShelfProgressData.SetShelfProgress(currentZoneIndex, nextShelf, rowInShelf);

        if (rowsStockedThisZone >= rowsPerZone)
        {
            if (currentBox != null)
            {
                BoxPickUp pickup = FindObjectOfType<BoxPickUp>();
                if (pickup != null)
                    pickup.ForceDropBox();

                Destroy(currentBox);
            }

            currentZoneIndex++;
            rowsStockedThisZone = 0;

            if (currentZoneIndex < stockZones.Count)
                StartCoroutine(SpawnNextBox());
        }
    }

    private IEnumerator SpawnNextBox()
    {
        yield return new WaitForSeconds(spawnDelay);
        SpawnBox();
    }

    public int GetCurrentZoneIndex() => currentZoneIndex;

    public void SetRowsStockedForZone(int zone, int rows)
    {
        if (zone == currentZoneIndex)
            rowsStockedThisZone = rows;
    }
}