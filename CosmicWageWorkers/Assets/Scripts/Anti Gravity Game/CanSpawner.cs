using UnityEngine;

public class CanSpawner : MonoBehaviour
{
    [Header("Spawning")]
    [SerializeField] private GameObject canPrefab;
    [SerializeField] private Transform[] spawnPoints = new Transform[3];
    [SerializeField] private Transform[] startPoints = new Transform[3];
    [SerializeField] private Transform[] endPoints = new Transform[3];
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private float spawnIntervalVariance = 0.3f;

    private float spawnTimer;

    private void Start()
    {
        spawnTimer = spawnInterval;
    }

    private void Update()
    {
        spawnTimer -= Time.deltaTime;

        if (spawnTimer <= 0f)
        {
            SpawnCan();
            spawnTimer = spawnInterval + Random.Range(-spawnIntervalVariance, spawnIntervalVariance);
        }
    }

    private void SpawnCan()
    {
        // Pick a random spawn point from the 3 rows
        int randomRow = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[randomRow];
        Transform startPoint = startPoints[randomRow];
        Transform endPoint = endPoints[randomRow];

        if (spawnPoint == null || startPoint == null || endPoint == null)
        {
            Debug.LogWarning("Row " + randomRow + " is missing spawn or path points!");
            return;
        }

        GameObject newCan = Instantiate(canPrefab, spawnPoint.position, Quaternion.identity);
        RollingCan rollingCan = newCan.GetComponent<RollingCan>();
        
        if (rollingCan != null)
        {
            rollingCan.SetPath(startPoint, endPoint);
            rollingCan.Initialize();
        }
    }
}
