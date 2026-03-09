using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FallingItemManager : MonoBehaviour
{
    [Header("Falling Items")]
    public GameObject fallingItemPrefab;
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

            if (playerAudio != null)
                playerAudio.PlayOneShot(playerAudio.warning);

            SpawnFallingItem();
        }
    }

    void SpawnFallingItem()
    {
        if (spawnPoints.Length == 0 || fallingItemPrefab == null || stopSpawning) return;

        int colIndex = Random.Range(0, spawnPoints.Length);
        Transform spawnPoint = spawnPoints[colIndex];

        GameObject item = Instantiate(fallingItemPrefab, spawnPoint.position, Quaternion.identity);

        // Flash warning icon twice
        if (warningIcon != null)
            StartCoroutine(FlashWarningTwice());

        StartCoroutine(HandleFallingItem(item));
    }

    private IEnumerator FlashWarningTwice()
    {
        if (warningIcon == null) yield break;

        Color c = warningIcon.color;

        for (int i = 0; i < 2; i++) // two flashes
        {
            c.a = 1f;
            warningIcon.color = c;
            yield return new WaitForSeconds(0.15f);

            c.a = 0f;
            warningIcon.color = c;
            yield return new WaitForSeconds(0.15f);
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
                // Player on same level
            }

            if (item.transform.position.y <= floorY)
            {
                Destroy(item);
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
                }
            }

            yield return null;
        }
    }

    public void StopSpawning()
    {
        stopSpawning = true;
    }
}