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

    private int maxMesses;
    private float displayedCleanlinessPercent = 100f;
    private float smoothSpeed = 3f;

    private void Start()
    {
        // Find managers if not assigned
        if (messManager == null)
            messManager = Object.FindFirstObjectByType<MessManager>();

        if (boxManager == null)
            boxManager = Object.FindFirstObjectByType<BoxManager>();

        // Subscribe to mess events
        if (messManager != null)
        {
            maxMesses = messManager.maxMessCount;
            messManager.OnMessCountChanged += OnMessCountChanged;
            UpdateCleanlinessUIInstant();
        }
    }

    private void OnDestroy()
    {
        if (messManager != null)
            messManager.OnMessCountChanged -= OnMessCountChanged;
    }

    private float targetCleanlinessPercent = 100f;

    private void OnMessCountChanged()
    {
        int currentMesses = messManager.activeMesses.Count;
        targetCleanlinessPercent = ((float)(maxMesses - currentMesses) / maxMesses) * 100f;
    }

    private void Update()
    {
        // Smooth cleanliness percentage
        displayedCleanlinessPercent = Mathf.Lerp(
            displayedCleanlinessPercent,
            targetCleanlinessPercent,
            Time.deltaTime * smoothSpeed
        );

        if (cleanlinessText != null)
            cleanlinessText.text = $"Store Cleanliness: {Mathf.RoundToInt(displayedCleanlinessPercent)}%";

        // Update stocking progress
        if (stockingText != null && boxManager != null)
        {
            int currentZone = boxManager.GetCurrentZoneIndex();
            int totalZones = boxManager.stockZones.Count;
            stockingText.text = $"Shelves Stocked: {currentZone}/{totalZones} Zones";
        }
    }

    private void UpdateCleanlinessUIInstant()
    {
        int currentMesses = messManager.activeMesses.Count;
        displayedCleanlinessPercent = ((float)(maxMesses - currentMesses) / maxMesses) * 100f;
        targetCleanlinessPercent = displayedCleanlinessPercent;

        if (cleanlinessText != null)
            cleanlinessText.text = $"Store Cleanliness: {Mathf.RoundToInt(displayedCleanlinessPercent)}%";
    }
}
