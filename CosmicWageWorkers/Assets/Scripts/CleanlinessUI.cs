using UnityEngine;
using TMPro;

public class CleanlinessUI : MonoBehaviour
{
    public TextMeshProUGUI cleanlinessText;
    public MessManager messManager;

    private int maxMesses;
    private float displayedPercent = 100f; // Current displayed percentage
    private float smoothSpeed = 3f; // How fast the number changes

    private void Start()
    {
        if (messManager == null)
            messManager = Object.FindFirstObjectByType<MessManager>();

        maxMesses = messManager.maxMessCount;

        // Subscribes to the MessManager events
        messManager.OnMessCountChanged += OnMessCountChanged;

        UpdateUIInstant(); // Set initial percentage
    }

    private void OnDestroy()
    {
        if (messManager != null)
            messManager.OnMessCountChanged -= OnMessCountChanged;
    }

    private float targetPercent = 100f;

    private void OnMessCountChanged()
    {
        // Calculates the new target cleanliness
        int currentMesses = messManager.activeMesses.Count;
        targetPercent = ((float)(maxMesses - currentMesses) / maxMesses) * 100f;
    }

    private void Update()
    {
        // Smoothly change the percentage on screen
        displayedPercent = Mathf.Lerp(displayedPercent, targetPercent, Time.deltaTime * smoothSpeed);

        if (cleanlinessText != null)
            cleanlinessText.text = $"Store Cleanliness: {Mathf.RoundToInt(displayedPercent)}%";
    }

    private void UpdateUIInstant()
    {
        int currentMesses = messManager.activeMesses.Count;
        displayedPercent = ((float)(maxMesses - currentMesses) / maxMesses) * 100f;
        targetPercent = displayedPercent;

        if (cleanlinessText != null)
            cleanlinessText.text = $"Store Cleanliness: {Mathf.RoundToInt(displayedPercent)}%";
    }
}
