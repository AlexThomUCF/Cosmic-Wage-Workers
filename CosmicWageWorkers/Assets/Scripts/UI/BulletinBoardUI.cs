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
    public WindowMessManager windowMessManager;

    [Header("UI Smoothing")]
    public float smoothSpeed = 3f;

    private int maxMesses;
    private float displayedCleanlinessPercent = 100f;
    private float targetCleanlinessPercent = 100f;

    private void Start()
    {
        if (messManager == null)
            messManager = Object.FindFirstObjectByType<MessManager>();
        if (boxManager == null)
            boxManager = Object.FindFirstObjectByType<BoxManager>();
        if (garbageManager == null)
            garbageManager = Object.FindFirstObjectByType<GarbageCanManager>();
        if (windowMessManager == null)
            windowMessManager = Object.FindFirstObjectByType<WindowMessManager>();

        if (messManager != null)
        {
            maxMesses = messManager.maxMessCount;
            messManager.OnMessCountChanged += OnCleanlinessChanged;
        }

        if (garbageManager != null)
            garbageManager.OnTrashCountChanged += OnCleanlinessChanged;

        if (windowMessManager != null)
            windowMessManager.OnWindowMessCountChanged += OnCleanlinessChanged;

        UpdateCleanlinessUIInstant();
    }

    private void OnDestroy()
    {
        if (messManager != null)
            messManager.OnMessCountChanged -= OnCleanlinessChanged;
        if (garbageManager != null)
            garbageManager.OnTrashCountChanged -= OnCleanlinessChanged;
        if (windowMessManager != null)
            windowMessManager.OnWindowMessCountChanged -= OnCleanlinessChanged;
    }

    private void OnCleanlinessChanged()
    {
        int currentMesses = messManager != null ? messManager.activeMesses.Count : 0;
        int currentGoo = windowMessManager != null ? windowMessManager.ActiveGooCount() : 0;
        int currentTrash = garbageManager != null ? garbageManager.ActiveTrashCount() : 0;

        int maxGoo = windowMessManager != null ? windowMessManager.windowSpawns.Length : 0;
        int maxTrash = garbageManager != null ? garbageManager.maxTrash : 0;

        int totalProblems = currentMesses + currentGoo + currentTrash;
        int maxProblems = maxMesses + maxGoo + maxTrash;

        if (maxProblems <= 0) maxProblems = 1; // avoid divide by zero

        targetCleanlinessPercent = Mathf.Clamp(
            ((float)(maxProblems - totalProblems) / maxProblems) * 100f,
            0f,
            100f
        );
    }

    private void Update()
    {
        displayedCleanlinessPercent = Mathf.Lerp(
            displayedCleanlinessPercent,
            targetCleanlinessPercent,
            Time.deltaTime * smoothSpeed
        );

        if (cleanlinessText != null)
            cleanlinessText.text = $"Store Cleanliness: {Mathf.RoundToInt(displayedCleanlinessPercent)}%";

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
        int currentGoo = windowMessManager != null ? windowMessManager.ActiveGooCount() : 0;
        int currentTrash = garbageManager != null ? garbageManager.ActiveTrashCount() : 0;

        int maxGoo = windowMessManager != null ? windowMessManager.windowSpawns.Length : 0;
        int maxTrash = garbageManager != null ? garbageManager.maxTrash : 0;

        int totalProblems = currentMesses + currentGoo + currentTrash;
        int maxProblems = maxMesses + maxGoo + maxTrash;

        if (maxProblems <= 0) maxProblems = 1;

        displayedCleanlinessPercent = Mathf.Clamp(
            ((float)(maxProblems - totalProblems) / maxProblems) * 100f,
            0f,
            100f
        );

        targetCleanlinessPercent = displayedCleanlinessPercent;

        if (cleanlinessText != null)
            cleanlinessText.text = $"Store Cleanliness: {Mathf.RoundToInt(displayedCleanlinessPercent)}%";
    }
}