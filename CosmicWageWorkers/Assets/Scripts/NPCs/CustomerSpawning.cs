using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawning : MonoBehaviour
{
    public GameObject npcPrefab;
    public Material[] materials; 
    public int npcCount = 10;
    public Transform[] waypoints;
    public Transform[] endWaypoints;
    public Transform entranceWaypoint;
    public Transform exitWaypoint;

    public float minSpawnDelay = 1f; // minimum time between spawns
    public float maxSpawnDelay = 4f; // maximum time between spawns

    void Start()
    {
        StartCoroutine(SpawnNPCs());
    }

    IEnumerator SpawnNPCs()
    {
        if (npcCount > waypoints.Length)
        {
            Debug.LogWarning("Not enough waypoints for the number of NPCs. Reducing npcCount to match waypoint count.");
            npcCount = waypoints.Length;
        }

        // Create a list of available waypoints
        List<Transform> availableWaypoints = new List<Transform>(waypoints);

        for (int i = 0; i < npcCount; i++)
        {
            // Pick a random waypoint from the available ones
            int index = Random.Range(0, availableWaypoints.Count);
            Transform spawnPoint = entranceWaypoint; //ALL npc spawn here

            // Spawn the NPC
            GameObject npc = Instantiate(npcPrefab, spawnPoint.position, Quaternion.identity);

            //Apply material to NPCs
            Material randomMat = materials[Random.Range(0, materials.Length)];
            SkinnedMeshRenderer smr = npc.GetComponentInChildren<SkinnedMeshRenderer>();
            smr.material = randomMat;

            // Assign waypoints to the NPC
            CustomerAI ai = npc.GetComponent<CustomerAI>();
            ai.waypoints = waypoints;
            ai.finalWaypoints = endWaypoints;
            ai.exitWaypoint = exitWaypoint;

            // Remove the used waypoint so no one else spawns here
            availableWaypoints.RemoveAt(index);

            // Wait a random amount of time before spawning the next NPC
            float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(delay);
        }
    }
}

