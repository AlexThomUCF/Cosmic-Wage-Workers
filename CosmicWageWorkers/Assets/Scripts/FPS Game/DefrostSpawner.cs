using UnityEngine;
using System.Collections.Generic;

public class DefrostSpawner : MonoBehaviour
{
    public Transform[] spawnPoints;
    public GameObject defrostObj;
    public float spawnDelay = 5f;

    private float timer;
    private Dictionary<Transform, GameObject> occupiedSpawns = new Dictionary<Transform, GameObject>();

    void Start()
    {
        foreach (Transform point in spawnPoints)
        {
            occupiedSpawns.Add(point, null);
        }
    }

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
        List<Transform> availablePoints = new List<Transform>();

        foreach (Transform point in spawnPoints)
        {
            if (occupiedSpawns[point] == null)
            {
                availablePoints.Add(point);
            }
        }

        if (availablePoints.Count == 0)
            return;

        Transform chosenPoint = availablePoints[Random.Range(0, availablePoints.Count)];

        GameObject spawned = Instantiate(
            defrostObj,
            chosenPoint.position + Vector3.up * 1f,
            chosenPoint.rotation
        );

        occupiedSpawns[chosenPoint] = spawned;
    }
}