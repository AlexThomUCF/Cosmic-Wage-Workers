using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FallingItemManager : MonoBehaviour
{
    [Header("Falling Items")]
    public GameObject[] fallingItemPrefabs; // changed to array
    public Transform[] spawnPoints;

    [Header("Timing")]
    public float minSpawnInterval = 10f;
    public float maxSpawnInterval = 20f;

    [Header("Audio")]
    public AudioSource warningAudio;

    [Header("References")]
    public Climbing playerClimbing;
    public Image warningIcon;

    [Header("Ground")]
    public float floorY = 0f;

    public PlayerAudio playerAudio;

    private bool stopSpawning = false;

    void Start()
    {
        if (warningIcon != null)
        {
            Color c = warningIcon.color;
            c.a = 0f;
            warningIcon.color = c;
        }

        StartCoroutine(SpawnFallingItemsRoutine());
    }

    IEnumerator SpawnFallingItemsRoutine()
    {
        while (!playerClimbing.isFalling)
        {
            if (stopSpawning) yield break;

            float waitTime = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(waitTime);

            if (stopSpawning) yield break;

            // Play warning sound
            if (playerAudio != null)
                playerAudio.PlayOneShot(playerAudio.warning);

            // Flash warning icon twice quickly
            if (warningIcon != null)
                StartCoroutine(FlashWarningIconTwice(0.15f)); // 0.15s per flash

            SpawnFallingItem();
        }
    }

    void SpawnFallingItem()
    {
        if (spawnPoints.Length == 0 || fallingItemPrefabs.Length == 0 || stopSpawning) return;

        int colIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[colIndex];

        int prefabIndex = Random.Range(0, fallingItemPrefabs.Length);
        GameObject chosenPrefab = fallingItemPrefabs[prefabIndex];

        GameObject item = Instantiate(chosenPrefab, spawnPoint.position, Quaternion.identity);

        StartCoroutine(HandleFallingItem(item));
    }

    IEnumerator FlashWarningIconTwice(float flashDuration)
    {
        Color c = warningIcon.color;

        for (int i = 0; i < 2; i++)
        {
            c.a = 1f;
            warningIcon.color = c;
            yield return new WaitForSeconds(flashDuration);

            c.a = 0f;
            warningIcon.color = c;
            yield return new WaitForSeconds(flashDuration);
        }
    }

    IEnumerator HandleFallingItem(GameObject item)
    {
        float fallSpeed = 12f;
        Vector3 spinAxis = Random.onUnitSphere;
        float spinSpeed = Random.Range(60f, 120f);

        float hitRadius = 1f;

        while (item != null)
        {
            item.transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            item.transform.Rotate(spinAxis, spinSpeed * Time.deltaTime, Space.World);

            if (item.transform.position.y <= playerClimbing.transform.position.y)
            {
                // player reached, stop showing warning if somehow still visible
                if (warningIcon != null)
                {
                    Color c = warningIcon.color;
                    c.a = 0f;
                    warningIcon.color = c;
                }
            }

            if (item.transform.position.y <= floorY)
            {
                Destroy(item);

                if (warningIcon != null)
                {
                    Color c = warningIcon.color;
                    c.a = 0f;
                    warningIcon.color = c;
                }
            }

            if (playerClimbing != null)
            {
                Vector3 horizontalDistance = item.transform.position - playerClimbing.transform.position;
                horizontalDistance.y = 0;

                if (item.transform.position.y <= playerClimbing.transform.position.y + 1f &&
                    horizontalDistance.magnitude <= hitRadius)
                {
                    if (playerAudio != null)
                        playerAudio.PlayOneShot(playerAudio.hitByItem);

                    playerClimbing.TriggerFall();
                    Destroy(item);

                    if (warningIcon != null)
                    {
                        Color c = warningIcon.color;
                        c.a = 0f;
                        warningIcon.color = c;
                    }
                }
            }

            yield return null;
        }

        if (warningIcon != null)
        {
            Color c = warningIcon.color;
            c.a = 0f;
            warningIcon.color = c;
        }
    }

    public void StopSpawning()
    {
        stopSpawning = true;
    }
}