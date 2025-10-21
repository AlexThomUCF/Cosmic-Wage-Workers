using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject rangedEnemyPrefab;
    public float spawnInterval = 5f;
    public Transform[] spawnPoints;
    public ParticleSystem portal;

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
        //Spawn portal particle here. 
        ParticleSystem clone;
        clone = Instantiate(portal, spawn.position, spawn.rotation);
        Instantiate(rangedEnemyPrefab, spawn.position, spawn.rotation);
        //Play sound
        //Spawn audio component on each portal, sound should be based on how close the player is to the enemy
        Destroy(clone, 3.5f);
    }

    
}

