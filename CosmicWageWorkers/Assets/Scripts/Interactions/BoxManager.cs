using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoxManager : MonoBehaviour
{
    [Header("Boxes & Shelves")]
    public GameObject boxPrefab;
    public List<Transform> shelfPositions; // The stockable shelves
    public float spawnHeight = 1f;
    public float spawnDelay = 2f;

    [Header("UI")]
    public TextMeshProUGUI shelfAlertText; // Shows which shelf the box corresponds to
    public TextMeshProUGUI shelfPercentageText; // Shows percentage of shelves stocked

    [Header("UI Settings")]
    public float lerpSpeed = 5f;

    private bool[] shelvesStocked;
    private int nextShelfIndex = 0;
    private GameObject currentBox;

    private float displayedPercentage = 0f;

    // Shows the current shelf index for ShelfStocking to check
    public int CurrentShelfIndex => nextShelfIndex;

    private void Start()
    {
        shelvesStocked = new bool[shelfPositions.Count];
        SpawnNextBox();
    }

    private void Update()
    {
        // Smoothly changes percentage on screen
        if (shelfPercentageText != null)
        {
            int stockedCount = 0;
            foreach (bool stocked in shelvesStocked)
                if (stocked) stockedCount++;

            float targetPercentage = ((float)stockedCount / shelvesStocked.Length) * 100f;
            displayedPercentage = Mathf.Lerp(displayedPercentage, targetPercentage, Time.deltaTime * lerpSpeed);
            shelfPercentageText.text = $"Shelves Stocked: {displayedPercentage:0}%";
        }
    }

    private void SpawnNextBox()
    {
        if (nextShelfIndex >= shelfPositions.Count)
        {
            shelfAlertText.text = "All shelves stocked!";
            return; // Stops spawning boxes
        }

        Vector3 spawnPos = GetRandomSpawnPosition();
        currentBox = Instantiate(boxPrefab, spawnPos + Vector3.up * spawnHeight, Quaternion.identity);

        // Tells the player which shelf to deliver the box to
        shelfAlertText.text = $"Deliver box to Shelf {nextShelfIndex + 1}";
    }

    private Vector3 GetRandomSpawnPosition() // Temporary position logic
    {
        float x = Random.Range(-10f, 10f);
        float z = Random.Range(-10f, 10f);
        return new Vector3(x, 0f, z);
    }

    public void OnShelfStocked(int shelfIndex)
    {
        if (shelfIndex != nextShelfIndex)
        {
            Debug.LogWarning("Attempted to stock wrong shelf. Ignored.");
            return;
        }

        // Destroys the box
        if (currentBox != null)
        {
            Destroy(currentBox);
            currentBox = null;
        }

        shelvesStocked[shelfIndex] = true;
        nextShelfIndex++;

        // Small delay before spawning next box
        StartCoroutine(SpawnNextBoxAfterDelay());
    }

    private IEnumerator SpawnNextBoxAfterDelay()
    {
        shelfAlertText.text = "Box stocked! Preparing next box...";
        yield return new WaitForSeconds(spawnDelay);
        SpawnNextBox();
    }
}