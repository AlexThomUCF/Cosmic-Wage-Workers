using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BoxManager : MonoBehaviour
{
    [Header("Boxes & Shelves")]
    public GameObject boxPrefab;
    public List<Transform> shelfPositions; // Assign 8 shelf positions
    public float spawnHeight = 1f;         // Height above ground to spawn box
    public float spawnDelay = 2f;          // Wait before spawning next box

    [Header("UI")]
    public TextMeshProUGUI shelfAlertText; // Shows which shelf the box corresponds to

    private bool[] shelvesStocked;
    private int nextShelfIndex = 0;
    private GameObject currentBox;

    // Expose the current shelf index for ShelfStocking to check
    public int CurrentShelfIndex => nextShelfIndex;

    private void Start()
    {
        shelvesStocked = new bool[shelfPositions.Count];
        SpawnNextBox();
    }

    private void SpawnNextBox()
    {
        if (nextShelfIndex >= shelfPositions.Count)
        {
            shelfAlertText.text = "All shelves stocked!";
            return; // Stop spawning boxes
        }

        Vector3 spawnPos = GetRandomSpawnPosition();
        currentBox = Instantiate(boxPrefab, spawnPos + Vector3.up * spawnHeight, Quaternion.identity);

        // Alert player which shelf to deliver the box to
        shelfAlertText.text = $"Deliver box to Shelf {nextShelfIndex + 1}";
    }

    private Vector3 GetRandomSpawnPosition()
    {
        // Replace with your preferred spawn logic or predefined points
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

        // Destroy the box
        if (currentBox != null)
        {
            Destroy(currentBox);
            currentBox = null;
        }

        shelvesStocked[shelfIndex] = true;
        nextShelfIndex++;

        // Brief delay before spawning next box
        StartCoroutine(SpawnNextBoxAfterDelay());
    }

    private IEnumerator SpawnNextBoxAfterDelay()
    {
        shelfAlertText.text = "Box stocked! Preparing next box...";
        yield return new WaitForSeconds(spawnDelay);
        SpawnNextBox();
    }
}
