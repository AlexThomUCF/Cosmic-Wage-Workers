using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class GarbageCanManager : MonoBehaviour
{
    [Header("Trash Settings")]
    public GameObject trashBagPrefab;       // assign your prefab
    public Transform[] garbageCanSpawns;    // 3 spawn points, one per can
    public float minRespawnTime = 5f;       // minimum seconds before a new bag spawns
    public float maxRespawnTime = 15f;      // maximum seconds before a new bag spawns

    [HideInInspector]
    public GameObject[] activeTrash;        // 1 per can

    public event Action OnTrashCountChanged;

    // Property for BulletinBoardUI
    public int maxTrash => garbageCanSpawns.Length;

    private void Awake()
    {
        activeTrash = new GameObject[garbageCanSpawns.Length];
    }

    private void Start()
    {
        StartCoroutine(RespawnLoop());
    }

    // Spawn a trash bag at a specific garbage can
    private void SpawnTrashAtCan(int index)
    {
        if (index < 0 || index >= garbageCanSpawns.Length) return;
        if (activeTrash[index] != null) return;

        GameObject bag = Instantiate(trashBagPrefab, garbageCanSpawns[index].position, Quaternion.identity);
        bag.transform.up = Vector3.up; // make sure upright

        // Disable physics initially to prevent freakouts
        Rigidbody rb = bag.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        // Collider enabled for raycast pickup
        Collider col = bag.GetComponent<Collider>();
        if (col != null) col.enabled = true;

        activeTrash[index] = bag;
        OnTrashCountChanged?.Invoke();
    }

    // Remove a trash bag (called when player picks it up)
    public void RemoveTrash(GameObject bag)
    {
        for (int i = 0; i < activeTrash.Length; i++)
        {
            if (activeTrash[i] == bag)
            {
                activeTrash[i] = null;
                Destroy(bag);
                OnTrashCountChanged?.Invoke();
                return;
            }
        }
    }

    // Returns current number of active trash bags
    public int ActiveTrashCount()
    {
        int count = 0;
        foreach (var bag in activeTrash)
            if (bag != null) count++;
        return count;
    }

    // Random respawn loop
    private IEnumerator RespawnLoop()
    {
        while (true)
        {
            float waitTime = UnityEngine.Random.Range(minRespawnTime, maxRespawnTime);
            yield return new WaitForSeconds(waitTime);

            // Pick a random can that doesn't have a bag
            List<int> freeIndices = new List<int>();
            for (int i = 0; i < activeTrash.Length; i++)
                if (activeTrash[i] == null) freeIndices.Add(i);

            if (freeIndices.Count == 0) continue;

            int chosenIndex = freeIndices[UnityEngine.Random.Range(0, freeIndices.Count)];
            SpawnTrashAtCan(chosenIndex);
        }
    }
}