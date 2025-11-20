using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoxManager : MonoBehaviour
{
    [Header("Boxes & Zones")]
    public GameObject boxPrefab;
    public List<Transform> stockZones;  // Each element = a stock zone root
    public float spawnHeight = 1f;
    public float spawnDelay = 2f;

    [Header("UI")]
    public TextMeshProUGUI alertText;
    public TextMeshProUGUI percentText;

    [Header("UI Settings")]
    public float lerpSpeed = 5f;

    private int currentZoneIndex = 0;
    private int rowsStockedThisZone = 0;
    private const int rowsPerZone = 8; // 4 shelves × 2 rows each

    private float displayedPercentage = 0f;
    private GameObject currentBox;

    private void Start()
    {
        UpdateUI();
        SpawnBox();
    }

    private void Update()
    {
        // Update percentage display (smooth)
        if (percentText != null)
        {
            float targetPercent = (float)currentZoneIndex / stockZones.Count * 100f;
            displayedPercentage = Mathf.Lerp(displayedPercentage, targetPercent, Time.deltaTime * lerpSpeed);
            percentText.text = $"Zones Stocked: {displayedPercentage:0}%";
        }
    }

    private void SpawnBox()
    {
        if (currentZoneIndex >= stockZones.Count)
        {
            alertText.text = "All Zones Fully Stocked!";
            return;
        }

        Vector3 pos = GetRandomSpawnPos();
        currentBox = Instantiate(boxPrefab, pos + Vector3.up * spawnHeight, Quaternion.identity);

        alertText.text = $"Deliver Box to Stock Zone {currentZoneIndex + 1}";
    }

    private Vector3 GetRandomSpawnPos()
    {
        float x = Random.Range(-10f, 10f);
        float z = Random.Range(-10f, 10f);
        return new Vector3(x, 0f, z);
    }

    // CALLED BY ShelfStocking when a single row is stocked
    public void OnRowStocked()
    {
        rowsStockedThisZone++;

        if (rowsStockedThisZone < rowsPerZone)
        {
            alertText.text = $"Zone {currentZoneIndex + 1}: {rowsStockedThisZone}/8 Rows Stocked";
            return;
        }

        // ZONE COMPLETED → delete box → move to next zone
        if (currentBox != null)
            Destroy(currentBox);

        currentZoneIndex++;
        rowsStockedThisZone = 0;

        alertText.text = "Zone Complete! Preparing next box...";

        StartCoroutine(SpawnNextBox());
    }

    private IEnumerator SpawnNextBox()
    {
        yield return new WaitForSeconds(spawnDelay);
        SpawnBox();
    }

    private void UpdateUI()
    {
        alertText.text = $"Deliver Box to Stock Zone {currentZoneIndex + 1}";
    }

    public int GetCurrentZoneIndex()
    {
        return currentZoneIndex;
    }
}