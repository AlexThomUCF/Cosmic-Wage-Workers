using UnityEngine;
using System.Collections;

public class FallingItemManager : MonoBehaviour
{
    [Header("Falling Items")]
    public GameObject fallingItemPrefab;
    public Transform[] spawnPoints; // One per column

    [Header("Timing")]
    public float minSpawnInterval = 10f;
    public float maxSpawnInterval = 20f;

    [Header("Audio")]
    public AudioSource warningAudio;

    [Header("References")]
    public Climbing playerClimbing; // Assign the player

    [Header("Ground")]
    public float floorY = 0f;

    void Start()
    {
        StartCoroutine(SpawnFallingItemsRoutine());
    }

    IEnumerator SpawnFallingItemsRoutine()
    {
        while (!playerClimbing.isFalling)
        {
            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);

            // Play audio cue
            if (warningAudio != null)
                warningAudio.Play();

            // Spawn item
            SpawnFallingItem();
        }
    }

    void SpawnFallingItem()
    {
        if (spawnPoints.Length == 0 || fallingItemPrefab == null) return;

        int colIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[colIndex];

        GameObject item = Instantiate(fallingItemPrefab, spawnPoint.position, Quaternion.identity);
        item.AddComponent<FallingItem>().Initialize(playerClimbing, floorY);
    }
}
