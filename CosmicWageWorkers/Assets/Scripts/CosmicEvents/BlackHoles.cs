using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHoles : MonoBehaviour
{
    [Header("Waypoints & Prefab")]
    public List<Transform> waypoints;
    public GameObject blackHolePrefab;

    [Header("Teleport Settings")]
    public float teleportCooldown = 1f;
    public float lifetime = 30f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip spawnDespawnClip;
    public AudioClip teleportClip;

    [Header("UI")]
    public CosmicPhenomenonUIManager uiManager;

    private GameObject player;
    private GameObject[] activeHoles = new GameObject[2];
    private bool canTeleport = true;

    private void Awake()
    {
        player = GameObject.Find("MainPlayer");
    }

    public void TriggerBlackHoles()
    {
        if (waypoints.Count < 2)
        {
            Debug.LogError("Not enough waypoints for Black Hole spawning!");
            return;
        }

        SpawnBlackHoles();

        if (audioSource != null && spawnDespawnClip != null)
            audioSource.PlayOneShot(spawnDespawnClip);

        if (uiManager != null) uiManager.ShowBlackHole(true);

        StartCoroutine(DestroyAfterTime(lifetime));
    }

    private void SpawnBlackHoles()
    {
        int indexA = Random.Range(0, waypoints.Count);
        int indexB;
        do { indexB = Random.Range(0, waypoints.Count); } while (indexB == indexA);

        activeHoles[0] = Instantiate(blackHolePrefab, waypoints[indexA].position, Quaternion.identity);
        activeHoles[1] = Instantiate(blackHolePrefab, waypoints[indexB].position, Quaternion.identity);

        foreach (var hole in activeHoles)
        {
            var trigger = hole.AddComponent<BlackHoleTrigger>();
            trigger.Setup(this);
        }
    }

    public void TeleportPlayer(GameObject enteredHole)
    {
        if (!canTeleport || player == null) return;

        GameObject destination = (enteredHole == activeHoles[0]) ? activeHoles[1] : activeHoles[0];
        player.transform.position = destination.transform.position;

        if (audioSource != null && teleportClip != null)
            audioSource.PlayOneShot(teleportClip);

        StartCoroutine(TeleportCooldownRoutine());
    }

    private IEnumerator TeleportCooldownRoutine()
    {
        canTeleport = false;
        yield return new WaitForSeconds(teleportCooldown);
        canTeleport = true;
    }

    private IEnumerator DestroyAfterTime(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        if (audioSource != null && spawnDespawnClip != null)
            audioSource.PlayOneShot(spawnDespawnClip);

        foreach (var hole in activeHoles)
            if (hole != null) Destroy(hole);

        if (uiManager != null) uiManager.ShowBlackHole(false);
    }
}

// Trigger component
public class BlackHoleTrigger : MonoBehaviour
{
    private BlackHoles manager;

    public void Setup(BlackHoles manager)
    {
        this.manager = manager;

        var col = gameObject.GetComponent<Collider>();
        if (col == null) col = gameObject.AddComponent<SphereCollider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            manager.TeleportPlayer(gameObject);
    }
}