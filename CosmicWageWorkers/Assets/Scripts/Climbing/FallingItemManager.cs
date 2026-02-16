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

    public PlayerAudio playerAudio;

    // Stop spawning when player hits the height trigger
    private bool stopSpawning = false;

    void Start()
    {
        StartCoroutine(SpawnFallingItemsRoutine());
    }

    IEnumerator SpawnFallingItemsRoutine()
    {
        while (!playerClimbing.isFalling)
        {
            if (stopSpawning)
            {
                // Exit the loop; existing items keep falling
                yield break;
            }

            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);

            if (stopSpawning) yield break;

            // Play audio cue
            playerAudio.PlayOneShot(playerAudio.warning);

            // Spawn item
            SpawnFallingItem();
        }
    }

    void SpawnFallingItem()
    {
        if (spawnPoints.Length == 0 || fallingItemPrefab == null || stopSpawning) return;

        int colIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[colIndex];

        GameObject item = Instantiate(fallingItemPrefab, spawnPoint.position, Quaternion.identity);
        item.AddComponent<FallingItem>().Initialize(playerClimbing, floorY);
    }

    // Called when the player hits the height trigger
    public void StopSpawning()
    {
        stopSpawning = true;
    }
}