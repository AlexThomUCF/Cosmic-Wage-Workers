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
    public AudioSource audioSource;        // AudioSource attached to manager
    public AudioClip spawnDespawnClip;    // Plays on spawn AND despawn
    public AudioClip teleportClip;        // Plays when player teleports

    private GameObject player;
    private GameObject[] activeHoles = new GameObject[2];
    private bool canTeleport = true;

    private void Awake()
    {
        player = GameObject.Find("MainPlayer");
    }

    /// <summary>
    /// Call this from the CosmicPhenomenonManager to spawn two black holes
    /// </summary>
    public void TriggerBlackHoles()
    {
        if (waypoints.Count < 2)
        {
            Debug.LogError("Not enough waypoints for Black Hole spawning!");
            return;
        }

        SpawnBlackHoles();

        // Play spawn sound
        if (audioSource != null && spawnDespawnClip != null)
            audioSource.PlayOneShot(spawnDespawnClip);

        StartCoroutine(DestroyAfterTime(lifetime));
    }

    private void SpawnBlackHoles()
    {
        // Pick two random distinct waypoints
        int indexA = Random.Range(0, waypoints.Count);
        int indexB;
        do
        {
            indexB = Random.Range(0, waypoints.Count);
        } while (indexB == indexA);

        // Instantiate black holes
        activeHoles[0] = Instantiate(blackHolePrefab, waypoints[indexA].position, Quaternion.identity);
        activeHoles[1] = Instantiate(blackHolePrefab, waypoints[indexB].position, Quaternion.identity);

        // Add trigger detection
        foreach (var hole in activeHoles)
        {
            var trigger = hole.AddComponent<BlackHoleTrigger>();
            trigger.Setup(this);
        }
    }

    public void TeleportPlayer(GameObject enteredHole)
    {
        if (!canTeleport || player == null) return;

        // Determine destination hole
        GameObject destination = (enteredHole == activeHoles[0]) ? activeHoles[1] : activeHoles[0];

        // Teleport
        player.transform.position = destination.transform.position;

        // Play teleport sound
        if (audioSource != null && teleportClip != null)
            audioSource.PlayOneShot(teleportClip);

        // Start cooldown
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

        // Play despawn sound
        if (audioSource != null && spawnDespawnClip != null)
            audioSource.PlayOneShot(spawnDespawnClip);

        foreach (var hole in activeHoles)
        {
            if (hole != null)
                Destroy(hole);
        }
    }
}

// Separate component to detect player entry
public class BlackHoleTrigger : MonoBehaviour
{
    private BlackHoles manager;

    public void Setup(BlackHoles manager)
    {
        this.manager = manager;

        // Ensure the black hole has a trigger collider
        var col = gameObject.GetComponent<Collider>();
        if (col == null) col = gameObject.AddComponent<SphereCollider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            manager.TeleportPlayer(gameObject);
        }
    }
}