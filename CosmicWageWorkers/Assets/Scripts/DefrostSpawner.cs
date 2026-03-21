using UnityEngine;

public class DefrostSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject defrostObj;
    public float spawnDelay = 5f;

    private float timer;

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= spawnDelay)
        {
            SpawnDefrost();
            timer = 0f;
        }
    }

    void SpawnDefrost()
    {
        int spawnCount = Random.Range(1, 3);

        for (int i = 0; i < spawnCount; i++)
        {
            int randomIndex = Random.Range(0, spawnPoints.Length);
            Transform spawnPoint = spawnPoints[randomIndex];

            Instantiate(defrostObj, spawnPoint.position + Vector3.up * 1f, spawnPoint.rotation);
        }
    }
}