using UnityEngine;
using System.Collections.Generic;

public class CustomerSpawning : MonoBehaviour
{
    public GameObject npcPrefab;
    public int npcCount = 10;
    public Transform[] waypoints;

    void Start()
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
            Transform spawnPoint = availableWaypoints[index];

            // Spawn the NPC
            GameObject npc = Instantiate(npcPrefab, spawnPoint.position, Quaternion.identity);

            // Assign waypoints to the NPC
            CustomerAI ai = npc.GetComponent<CustomerAI>();
            ai.waypoints = waypoints;

            // Remove the used waypoint so no one else spawns here
            availableWaypoints.RemoveAt(index);
        }
    }
}

