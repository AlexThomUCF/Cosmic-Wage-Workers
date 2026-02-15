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
    public bool spawningEnabled = true;

    void Start()
    {
        RecalculateSpawnRate();
        StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            if (spawningEnabled)
            {
                SpawnOne();
            }

            yield return new WaitForSeconds(currentSpawnRate);
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

    // ====== CALLED BY ALARM / GAME MANAGER ======

    public void IncreasePressure(float amount)
    {
        spawnRateMultiplier -= amount;
        spawnRateMultiplier = Mathf.Clamp(spawnRateMultiplier, minMultiplier, maxMultiplier);
        RecalculateSpawnRate();

        Debug.Log($"[SPAWN] Pressure increased → x{spawnRateMultiplier}");
    }

    public void DecreasePressure(float amount)
    {
        spawnRateMultiplier += amount;
        spawnRateMultiplier = Mathf.Clamp(spawnRateMultiplier, minMultiplier, maxMultiplier);
        RecalculateSpawnRate();

        Debug.Log($"[SPAWN] Pressure decreased → x{spawnRateMultiplier}");
    }

    void RecalculateSpawnRate()
    {
        currentSpawnRate = baseSpawnRate * spawnRateMultiplier;
    }

    public void PauseSpawning(float duration)
    {
        StartCoroutine(PauseCoroutine(duration));
    }

    IEnumerator PauseCoroutine(float duration)
    {
        spawningEnabled = false;
        yield return new WaitForSeconds(duration);
        spawningEnabled = true;
    }

    public void SetPressureNormalized(float pressure01)
    {
        // pressure01: 0 = calm, 1 = max panic
        spawnRateMultiplier = Mathf.Lerp(maxMultiplier, minMultiplier, pressure01);
        RecalculateSpawnRate();

        Debug.Log($"[SPAWN] Pressure {pressure01:F2} → multiplier {spawnRateMultiplier:F2} → rate {currentSpawnRate:F2}s");
    }

}
