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
    public Climbing playerClimbing; // player reference
    public Image warningIcon;       // assign your UI Image from Canvas

    [Header("Ground")]
    public float floorY = 0f;

    public PlayerAudio playerAudio;
    
    private bool stopSpawning = false;

    void Start()
    {
        if (warningIcon != null)
            warningIcon.enabled = false;

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

        StartCoroutine(HandleFallingItem(item));
    }

    IEnumerator HandleFallingItem(GameObject item)
    {
        float fallSpeed = 12f;
        Vector3 spinAxis = Random.onUnitSphere;
        float spinSpeed = Random.Range(60f, 120f);

        // Faster flash for sync with sound
        float flashInterval = 0.25f; // flashes every 0.25s
        float flashTimer = flashInterval;

        float hitRadius = 1f;

        // Enable the icon at top-middle
        if (warningIcon != null)
        {
            warningIcon.enabled = true;

            RectTransform rt = warningIcon.rectTransform;
            rt.anchorMin = new Vector2(0.5f, 1f);
            rt.anchorMax = new Vector2(0.5f, 1f);
            rt.pivot = new Vector2(0.5f, 1f);
            rt.anchoredPosition = new Vector2(0f, -50f); // 50 units down from top
            rt.localScale = Vector3.one;
        }

        while (item != null)
        {
            // Move and spin the item
            item.transform.position += Vector3.down * fallSpeed * Time.deltaTime;
            item.transform.Rotate(spinAxis, spinSpeed * Time.deltaTime, Space.World);

            // Flash the icon
            if (warningIcon != null && warningIcon.enabled)
            {
                flashTimer -= Time.deltaTime;
                if (flashTimer <= 0f)
                {
                    warningIcon.enabled = !warningIcon.enabled;
                    flashTimer = flashInterval;
                }
            }

            // Hide icon when item passes player
            if (item.transform.position.y <= playerClimbing.transform.position.y)
            {
                if (warningIcon != null) warningIcon.enabled = false;
            }

            // Destroy item if hits floor
            if (item.transform.position.y <= floorY)
            {
                Destroy(item);
                if (warningIcon != null) warningIcon.enabled = false;
            }

            // Player collision with horizontal check
            if (playerClimbing != null)
            {
                Vector3 horizontalDistance = item.transform.position - playerClimbing.transform.position;
                horizontalDistance.y = 0;

                if (item.transform.position.y <= playerClimbing.transform.position.y + 1f &&
                    horizontalDistance.magnitude <= hitRadius)
                {
                    if (playerAudio != null) playerAudio.PlayOneShot(playerAudio.hitByItem);
                    playerClimbing.TriggerFall();
                    Destroy(item);
                    if (warningIcon != null) warningIcon.enabled = false;
                }
            }

            yield return null;
        }

        if (warningIcon != null) warningIcon.enabled = false;
    }

    public void StopSpawning()
    {
        stopSpawning = true;
    }
}