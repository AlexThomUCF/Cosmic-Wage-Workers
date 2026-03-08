using UnityEngine;
using TMPro;

public class BulletinBoardUI : MonoBehaviour
{
    [Header("Text References")]
    public TextMeshProUGUI cleanlinessText;
    public TextMeshProUGUI stockingText;

    [Header("Managers")]
    public MessManager messManager;
    public BoxManager boxManager;
    public GarbageCanManager garbageManager;

    private int maxMesses;
    private float displayedCleanlinessPercent = 100f;
    private float smoothSpeed = 3f;
    private float targetCleanlinessPercent = 100f;

    private void Start()
    {
        if (messManager == null)
            messManager = Object.FindFirstObjectByType<MessManager>();

        if (boxManager == null)
            boxManager = Object.FindFirstObjectByType<BoxManager>();

        if (garbageManager == null)
            garbageManager = Object.FindFirstObjectByType<GarbageCanManager>();

        // Subscribe to mess events
        if (messManager != null)
        {
            maxMesses = messManager.maxMessCount;
            messManager.OnMessCountChanged += OnCleanlinessChanged;
        }

        // Subscribe to garbage events
        if (garbageManager != null)
        {
            garbageManager.OnTrashCountChanged += OnCleanlinessChanged;
        }

        UpdateCleanlinessUIInstant();
    }

    private void OnDestroy()
    {
        if (messManager != null)
            messManager.OnMessCountChanged -= OnCleanlinessChanged;

        if (garbageManager != null)
            garbageManager.OnTrashCountChanged -= OnCleanlinessChanged;
    }

    private void OnCleanlinessChanged()
    {
        int currentMesses = messManager != null ? messManager.activeMesses.Count : 0;
        int currentTrash = garbageManager != null ? garbageManager.ActiveTrashCount() : 0;

        int maxTrash = garbageManager != null ? garbageManager.maxTrash : 0;

        int totalProblems = currentMesses + currentTrash;
        int maxProblems = maxMesses + maxTrash;

        if (maxProblems > 0)
            targetCleanlinessPercent = ((float)(maxProblems - totalProblems) / maxProblems) * 100f;
        else
            targetCleanlinessPercent = 100f; // avoid divide by zero
    }

    private void Update()
    {
        // Smoothly lerp to target cleanliness
        displayedCleanlinessPercent = Mathf.Lerp(
            displayedCleanlinessPercent,
            targetCleanlinessPercent,
            Time.deltaTime * smoothSpeed
        );

        if (cleanlinessText != null)
            cleanlinessText.text = $"Store Cleanliness: {Mathf.RoundToInt(displayedCleanlinessPercent)}%";

        // Update stocking UI
        if (stockingText != null && boxManager != null)
        {
            int currentZone = boxManager.GetCurrentZoneIndex();
            int totalZones = boxManager.stockZones.Count;

            stockingText.text = $"Shelves Stocked: {currentZone}/{totalZones} Zones";
        }
    }

    private void UpdateCleanlinessUIInstant()
    {
        int currentMesses = messManager != null ? messManager.activeMesses.Count : 0;
        int currentTrash = garbageManager != null ? garbageManager.ActiveTrashCount() : 0;

        int maxTrash = garbageManager != null ? garbageManager.maxTrash : 0;

        int totalProblems = currentMesses + currentTrash;
        int maxProblems = maxMesses + maxTrash;

        if (maxProblems > 0)
            displayedCleanlinessPercent = ((float)(maxProblems - totalProblems) / maxProblems) * 100f;
        else
            displayedCleanlinessPercent = 100f;

        targetCleanlinessPercent = displayedCleanlinessPercent;

        if (cleanlinessText != null)
            cleanlinessText.text = $"Store Cleanliness: {Mathf.RoundToInt(displayedCleanlinessPercent)}%";
    }
}