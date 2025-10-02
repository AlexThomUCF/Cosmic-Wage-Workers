using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject rangedEnemyPrefab;
    public GameObject meleeEnemyPrefab;
    public float spawnInterval = 5f;
    public Transform[] spawnPoints;

    private float timer;

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0f)
        {
            timer = spawnInterval;
            SpawnEnemy();
        }
    }

    void SpawnEnemy()
    {
        if (spawnPoints.Length == 0) return;

        Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject prefab = Random.value > 0.5f ? rangedEnemyPrefab : meleeEnemyPrefab;
        Instantiate(prefab, spawn.position, spawn.rotation);
    }
}

