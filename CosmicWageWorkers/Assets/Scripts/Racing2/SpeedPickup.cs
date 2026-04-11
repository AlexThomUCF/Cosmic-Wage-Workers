using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpeedPickup : MonoBehaviour
{
    public PlayerMovement player;

    [SerializeField] private float boostedSpeed = 20f;
    [SerializeField] private float duration = 5f;

    private Collider pickupCollider;
    private Renderer pickupRenderer;
    private bool collected;

    // Stores each player's true original speed
    private static Dictionary<PlayerMovement, float> originalSpeeds = new Dictionary<PlayerMovement, float>();

    private void Awake()
    {
        player = FindAnyObjectByType<PlayerMovement>();
        pickupCollider = GetComponent<Collider>();
        pickupRenderer = GetComponent<Renderer>();

        if (player != null && !originalSpeeds.ContainsKey(player))
        {
            originalSpeeds[player] = player.moveSpeed;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (collected) return;

        if (!other.CompareTag("Player")) return;

        collected = true;
        StartCoroutine(SpeedBoost());
    }

    private IEnumerator SpeedBoost()
    {
        if (player == null) yield break;

        if (pickupCollider != null) pickupCollider.enabled = false;
        if (pickupRenderer != null) pickupRenderer.enabled = false;

        player.moveSpeed = boostedSpeed;

        yield return new WaitForSeconds(duration);

        if (originalSpeeds.ContainsKey(player))
        {
            player.moveSpeed = originalSpeeds[player];
        }

        Destroy(gameObject);
    }
}