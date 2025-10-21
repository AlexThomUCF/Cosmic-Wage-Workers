using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MessManager : MonoBehaviour
{
    public GameObject messPrefab;
    public int maxMessCount = 5;
    public float respawnDelay = 3f;

    [HideInInspector] public List<GameObject> activeMesses = new List<GameObject>();
    private List<Transform> spawnPoints = new List<Transform>();
    private HashSet<Transform> occupiedPoints = new HashSet<Transform>();

    public event System.Action OnMessCountChanged; // NEW EVENT

    private void Awake()
    {
        GameObject[] points = GameObject.FindGameObjectsWithTag("MessSpawnPoint");
        foreach (var p in points)
            spawnPoints.Add(p.transform);
    }

    private void Start()
    {
        for (int i = 0; i < maxMessCount; i++)
            SpawnMess();

        StartCoroutine(RespawnLoop());
    }

    private void SpawnMess()
    {
        List<Transform> freePoints = new List<Transform>();
        foreach (var t in spawnPoints)
        {
            if (!occupiedPoints.Contains(t))
                freePoints.Add(t);
        }

        if (freePoints.Count == 0) return;

        Transform spawnPoint = freePoints[Random.Range(0, freePoints.Count)];

        GameObject mess = Instantiate(messPrefab, spawnPoint.position, Quaternion.identity);
        mess.GetComponent<FloorCleaning>().OnMessCleaned += HandleMessCleaned;
        activeMesses.Add(mess);
        occupiedPoints.Add(spawnPoint);

        OnMessCountChanged?.Invoke(); // Notify UI
    }

    private void HandleMessCleaned(GameObject cleanedMess)
    {
        activeMesses.Remove(cleanedMess);

        Transform spawnPoint = spawnPoints.Find(p => Vector3.Distance(p.position, cleanedMess.transform.position) < 0.1f);
        if (spawnPoint != null)
            occupiedPoints.Remove(spawnPoint);

        Destroy(cleanedMess);

        OnMessCountChanged?.Invoke(); // Notify UI
    }

    private IEnumerator RespawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(respawnDelay);
            if (activeMesses.Count < maxMessCount)
                SpawnMess();
        }
    }
}
