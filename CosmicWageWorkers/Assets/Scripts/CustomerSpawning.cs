using UnityEngine;

public class CustomerSpawning : MonoBehaviour
{
    public GameObject npcPrefab;
    public int npcCount = 10;
    public Transform[] waypoints;

    void Start()
    {
        for (int i = 0; i < npcCount; i++)
        {
            Vector3 spawnPos = waypoints[Random.Range(0, waypoints.Length)].position;
            GameObject npc = Instantiate(npcPrefab, spawnPos, Quaternion.identity);

            // Assign waypoints to the NPC
            CustomerAI ai = npc.GetComponent<CustomerAI>();
            ai.waypoints = waypoints;
        }
    }
}
