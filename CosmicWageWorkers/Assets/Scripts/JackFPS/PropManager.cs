using System.Collections;
using UnityEngine;

public class PropManager : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform[] locations;
    public GameObject[] shelfItems;

    [Header("Spawn Settings")]
    public float baseSpawnRate = 0.75f;
    private float currentSpawnRate;

    public bool spawningEnabled = true;

    void Start()
    {
        currentSpawnRate = baseSpawnRate;
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

        LookAtPlayer lookScript = spawnedObj.GetComponent<LookAtPlayer>();
        if (lookScript != null)
        {
            lookScript.player = player;
        }
    }

    // ====== CALLED BY GAME MANAGER ======

    public void ModifySpawnRate(float multiplier)
    {
        currentSpawnRate = baseSpawnRate * multiplier;
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
}

