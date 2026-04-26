using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowMessManager : MonoBehaviour
{
    [Header("Settings")]
    public List<GameObject> gooPrefabs;
    public Transform[] windowSpawns;

    public int gooPerMess = 3;
    public float minSpawnDelay = 8f;
    public float maxSpawnDelay = 20f;

    [Header("Goo Variation")]
    public float minScale = 0.7f;
    public float maxScale = 1.3f;

    public float minDistanceBetweenGoo = 0.4f;

    [HideInInspector] public List<GameObject> activeGoo = new List<GameObject>();

    private HashSet<Transform> dirtyWindows = new HashSet<Transform>();

    public event System.Action OnWindowMessCountChanged;

    private void Start()
    {
        StartCoroutine(RandomSpawnLoop());
    }

    private void SpawnGoo(Transform window)
    {
        for (int i = 0; i < gooPerMess; i++)
        {
            GameObject prefab = gooPrefabs[Random.Range(0, gooPrefabs.Count)];

            Vector3 spawnPos = Vector3.zero;
            bool validPosition = false;

            // Try several times to find a non-overlapping spot
            for (int attempt = 0; attempt < 10; attempt++)
            {
                Vector3 localOffset = new Vector3(
                    -0.2f,
                    Random.Range(-2f, 2f),
                    Random.Range(-1f, 1f)
                );

                spawnPos = window.TransformPoint(localOffset);

                validPosition = true;

                foreach (var g in activeGoo)
                {
                    if (g == null) continue;

                    if (Vector3.Distance(g.transform.position, spawnPos) < minDistanceBetweenGoo)
                    {
                        validPosition = false;
                        break;
                    }
                }

                if (validPosition)
                    break;
            }

    // Spawn goo
    GameObject goo = Instantiate(prefab, spawnPos, Quaternion.Euler(0f, 90f, Random.Range(0f, 360f)));

    // Random scale
    float randomScale = Random.Range(minScale, maxScale);
    goo.transform.localScale = Vector3.one * randomScale;

    activeGoo.Add(goo);
}

        dirtyWindows.Add(window);

        OnWindowMessCountChanged?.Invoke();
    }

    private IEnumerator RandomSpawnLoop()
    {
        while (true)
        {
            float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(delay);

            if (dirtyWindows.Count >= windowSpawns.Length)
                continue;

            List<Transform> cleanWindows = new List<Transform>();

            foreach (var window in windowSpawns)
            {
                if (!dirtyWindows.Contains(window))
                    cleanWindows.Add(window);
            }

            if (cleanWindows.Count > 0)
            {
                Transform randomWindow = cleanWindows[Random.Range(0, cleanWindows.Count)];
                SpawnGoo(randomWindow);
            }
        }
    }

    public void RemoveGoo(GameObject goo)
    {
        if (activeGoo.Contains(goo))
            activeGoo.Remove(goo);

        Destroy(goo);

        foreach (var window in windowSpawns)
        {
            bool windowStillDirty = false;

            foreach (var g in activeGoo)
            {
                if (g != null && Vector3.Distance(g.transform.position, window.position) < 1.5f)
                {
                    windowStillDirty = true;
                    break;
                }
            }

            if (!windowStillDirty)
                dirtyWindows.Remove(window);
        }

        FindObjectOfType<CustomerManager>()?.OnTaskCompleted();

        OnWindowMessCountChanged?.Invoke();
    }

    public int ActiveGooCount()
    {
        return activeGoo.Count;
    }
}