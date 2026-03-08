using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowMessManager : MonoBehaviour
{
    [Header("Settings")]
    public List<GameObject> gooPrefabs;          // List of different goo prefabs
    public Transform[] windowSpawns;             // One per window
    public int maxGooPerWindow = 3;              // Max active goo per window
    public int gooPerSpawn = 2;                  // How many pieces to spawn at once
    public float respawnDelay = 5f;              // Seconds between spawn attempts

    [HideInInspector] public List<GameObject> activeGoo = new List<GameObject>();

    public event System.Action OnWindowMessCountChanged;

    private void Start()
    {
        StartCoroutine(RespawnLoop());
    }

    private void SpawnGoo(Transform window)
    {
        // Count existing goo on this window
        int count = 0;
        foreach (var g in activeGoo)
        {
            if (g != null && Vector3.Distance(g.transform.position, window.position) < 0.1f)
                count++;
        }

        int toSpawn = Mathf.Min(gooPerSpawn, maxGooPerWindow - count);
        for (int i = 0; i < toSpawn; i++)
        {
            // Pick a random prefab from the list
            GameObject prefab = gooPrefabs[Random.Range(0, gooPrefabs.Count)];
            Vector3 offset = new Vector3(Random.Range(-0.3f, 0.3f), Random.Range(-0.3f, 0.3f), 0f);
            GameObject goo = Instantiate(prefab, window.position + offset, Quaternion.identity);
            goo.transform.up = Vector3.up;
            activeGoo.Add(goo);
        }

        if (toSpawn > 0)
            OnWindowMessCountChanged?.Invoke();
    }

    private IEnumerator RespawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(respawnDelay);

            foreach (var window in windowSpawns)
            {
                SpawnGoo(window);
            }
        }
    }

    public void RemoveGoo(GameObject goo)
    {
        if (activeGoo.Contains(goo))
            activeGoo.Remove(goo);

        Destroy(goo);
        OnWindowMessCountChanged?.Invoke();
    }

    public int ActiveGooCount()
    {
        return activeGoo.Count;
    }
}