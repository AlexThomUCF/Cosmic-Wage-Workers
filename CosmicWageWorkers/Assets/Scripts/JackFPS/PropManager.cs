using System.Collections;
using UnityEngine;

public class PropManager : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform[] locations;
    public Transform[] exitPoints;
    public GameObject[] shelfItems;

    [Header("Spawn Settings")]
    public float baseSpawnRate = 0.75f;

    [Tooltip("Lower = harder (faster spawns), Higher = easier")]
    [SerializeField] private float spawnRateMultiplier = 1f;

    [SerializeField] private float minMultiplier = 0.4f;
    [SerializeField] private float maxMultiplier = 2f;

    private float currentSpawnRate;
    public bool spawningEnabled = false;   // ← START DISABLED

    private Coroutine spawnRoutine;

    void Start()
    {
        RecalculateSpawnRate();
        // DO NOT start spawning automatically
    }

    // ===== SPAWN CONTROL =====

    public void StartSpawning()
    {
        if (spawnRoutine == null)
        {
            spawningEnabled = true;
            spawnRoutine = StartCoroutine(SpawnLoop());
        }
    }

    public void StopSpawning()
    {
        spawningEnabled = false;

        if (spawnRoutine != null)
        {
            StopCoroutine(spawnRoutine);
            spawnRoutine = null;
        }
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(currentSpawnRate);

            if (spawningEnabled)
            {
                SpawnOne();
            }
        }
    }

    void SpawnOne()
    {
        Transform spawnPoint = locations[Random.Range(0, locations.Length)];
        GameObject prefab = shelfItems[Random.Range(0, shelfItems.Length)];

        GameObject spawnedObj = Instantiate(prefab, spawnPoint.position, Quaternion.identity);

        PropAi ai = spawnedObj.GetComponent<PropAi>();
        if (ai != null && exitPoints.Length > 0)
        {
            ai.exit = exitPoints[Random.Range(0, exitPoints.Length)];
        }

        LookAtPlayer lookScript = spawnedObj.GetComponent<LookAtPlayer>();
        if (lookScript != null)
        {
            lookScript.player = player;
        }
    }

    void RecalculateSpawnRate()
    {
        currentSpawnRate = baseSpawnRate * spawnRateMultiplier;
    }

    // ====== PRESSURE SYSTEM (unchanged) ======

    public void IncreasePressure(float amount)
    {
        spawnRateMultiplier -= amount;
        spawnRateMultiplier = Mathf.Clamp(spawnRateMultiplier, minMultiplier, maxMultiplier);
        RecalculateSpawnRate();
    }

    public void DecreasePressure(float amount)
    {
        spawnRateMultiplier += amount;
        spawnRateMultiplier = Mathf.Clamp(spawnRateMultiplier, minMultiplier, maxMultiplier);
        RecalculateSpawnRate();
    }

    public void SetPressureNormalized(float pressure01)
    {
        spawnRateMultiplier = Mathf.Lerp(maxMultiplier, minMultiplier, pressure01);
        RecalculateSpawnRate();
    }
}
