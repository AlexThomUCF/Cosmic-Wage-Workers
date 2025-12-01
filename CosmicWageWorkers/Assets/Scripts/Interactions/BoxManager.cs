using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoxManager : MonoBehaviour
{
    [Header("Boxes & Zones")]
    public GameObject boxPrefab;
    public List<Transform> stockZones;
    public float spawnHeight = 1f;
    public float spawnDelay = 1f;

    [Header("UI")]
    public TextMeshProUGUI percentText;

    [Header("UI Settings")]
    public float lerpSpeed = 5f;

    private int currentZoneIndex = 0;
    private int rowsStockedThisZone = 0;
    private const int rowsPerZone = 8; // 4 shelves × 2 rows

    private GameObject currentBox;

    private void Start()
    {
        currentZoneIndex = 0;

        // Restore saved progress across all zones
        for (int z = 0; z < stockZones.Count; z++)
        {
            int stockedRows = ShelfProgressData.GetRowsStockedThisZone(z);
            if (stockedRows >= rowsPerZone)
                currentZoneIndex = Mathf.Max(currentZoneIndex, z + 1);
        }

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
        if (currentZoneIndex >= stockZones.Count) return;

        Vector3 pos = GetRandomSpawnPos();
        currentBox = Instantiate(boxPrefab, pos + Vector3.up * spawnHeight, Quaternion.identity);

        currentBox.tag = "Box";
        if (currentBox.GetComponent<Collider>() == null)
            currentBox.AddComponent<BoxCollider>();

        if (currentBox.GetComponent<Rigidbody>() == null)
        {
            Rigidbody rb = currentBox.AddComponent<Rigidbody>();
            rb.isKinematic = false;
        }
    }

    private Vector3 GetRandomSpawnPos()
    {
        float x = Random.Range(-10f, 10f);
        float z = Random.Range(-10f, 10f);
        return new Vector3(x, 0f, z);
    }

    public void OnRowStocked()
    {
        rowsStockedThisZone++;
        ShelfProgressData.SetShelfProgress(currentZoneIndex, rowsStockedThisZone / 2, rowsStockedThisZone % 2);

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