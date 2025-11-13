using UnityEngine;

[System.Serializable]
public class CustomerInteraction
{
    [Tooltip("Unique ID for this interaction (example: 'FPS', 'Puzzle', etc.)")]
    public string interactionID;

    [Tooltip("Prefab of the customer for this interaction")]
    public GameObject customerPrefab;
}