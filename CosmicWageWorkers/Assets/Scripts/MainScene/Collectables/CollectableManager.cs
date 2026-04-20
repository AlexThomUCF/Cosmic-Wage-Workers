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

    public void Collect(string id)
    {
        collectedIDs.Add(id);
    }

    public bool IsCollected(string id)
    {
        return collectedIDs.Contains(id);
    }

    public int GetCollectedCount()
    {
        return collectedIDs.Count;
    }
}
