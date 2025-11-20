using System.Collections;
using UnityEngine;

public class PrimordialSoup : MonoBehaviour
{
    [Header("Settings")]
    public float speedDuration = 20f;
    public float cleaningSpeedMultiplier = 0.5f; // reduces clean time by 50%

    [Header("Spawn Settings")]
    public GameObject soupPrefab;
    public Transform[] spawnPoints;

    [Header("UI")]
    public CosmicPhenomenonUIManager uiManager;

    private GameObject activeSoup;
    private bool eventActive = false;

    public static PrimordialSoup Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    public void TriggerSoup()
    {
        if (eventActive || spawnPoints.Length == 0 || soupPrefab == null) return;

        eventActive = true;

        // Pick a random spawn point
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        // Spawn the soup puddle
        activeSoup = Instantiate(soupPrefab, spawnPoint.position, Quaternion.identity);

        // Subscribe to interaction
        InteractableObject interactable = activeSoup.GetComponent<InteractableObject>();
        if (interactable != null)
        {
            interactable.onInteract.AddListener(StartSpeedBoost);
        }
    }

    public void StartSpeedBoost()
    {
        // Remove soup object
        if (activeSoup != null)
        {
            Destroy(activeSoup);
            activeSoup = null;
        }

        // Apply speed boost to all active FloorCleaning instances
        FloorCleaning[] cleanings = FindObjectsOfType<FloorCleaning>();
        foreach (var cleaning in cleanings)
        {
            cleaning.cleanTimePerPiece *= cleaningSpeedMultiplier;
        }

        // Apply speed boost to ShelfStocking
        ShelfStocking[] shelves = FindObjectsOfType<ShelfStocking>();
        foreach (var shelf in shelves)
        {
            shelf.rowCooldown *= cleaningSpeedMultiplier;
        }

        // Show UI icon
        if (uiManager != null)
            uiManager.ShowPrimordialSoup(true);

        StartCoroutine(EndSpeedBoostAfterDelay());
    }

    private IEnumerator EndSpeedBoostAfterDelay()
    {
        float timer = 0f;

        while (timer < speedDuration)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        // Reset FloorCleaning speeds
        FloorCleaning[] cleanings = FindObjectsOfType<FloorCleaning>();
        foreach (var cleaning in cleanings)
        {
            cleaning.cleanTimePerPiece /= cleaningSpeedMultiplier;
        }

        // Reset ShelfStocking speeds
        ShelfStocking[] shelves = FindObjectsOfType<ShelfStocking>();
        foreach (var shelf in shelves)
        {
            shelf.rowCooldown /= cleaningSpeedMultiplier;
        }

        // Hide UI icon
        if (uiManager != null)
            uiManager.ShowPrimordialSoup(false);

        eventActive = false;
    }
}