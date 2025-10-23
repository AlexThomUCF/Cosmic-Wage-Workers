using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject rangedEnemyPrefab;
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
        if (spawnPoints.Length == 0 || rangedEnemyPrefab == null) return;

        Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(rangedEnemyPrefab, spawn.position, spawn.rotation);
    }
}

