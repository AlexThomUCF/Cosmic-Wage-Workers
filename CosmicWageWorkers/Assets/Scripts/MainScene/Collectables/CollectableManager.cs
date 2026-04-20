using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager Instance;

    private HashSet<string> collectedIDs = new HashSet<string>();

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // CALLED BY GAME
    public void Collect(string id)
    {
        collectedIDs.Add(id);
    }

    public bool IsCollected(string id)
    {
        return collectedIDs.Contains(id);
    }

    // for saving
    public List<string> GetAllCollectedIDs()
    {
        return new List<string>(collectedIDs);
    }

    // for loading
    public void SetCollectedIDs(List<string> ids)
    {
        collectedIDs = new HashSet<string>(ids);

        // refresh shelf after load
        FindFirstObjectByType<CollectableShelf>()?.RefreshShelf();
    }
}