using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerSpawning : MonoBehaviour
{
    public CustomerPool pool;

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
        for (int i = 0; i < npcCount; i++)
        {
            SpawnNPC();

            float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(delay);
        }
    }

    public void SpawnNPC()
    {
        GameObject npc = pool.GetCustomer();

        npc.transform.SetPositionAndRotation(
         entranceWaypoint.position,
         entranceWaypoint.rotation);

        // Apply random material
        Material randomMat = materials[Random.Range(0, materials.Length)];
        SkinnedMeshRenderer smr = npc.GetComponentInChildren<SkinnedMeshRenderer>();
        smr.material = randomMat;

        // Assign AI
        CustomerAI ai = npc.GetComponent<CustomerAI>();

        ai.ResetAgent(entranceWaypoint.position);

        ai.waypoints = waypoints;
        ai.finalWaypoints = endWaypoints;
        ai.exitWaypoint = exitWaypoint;

        ai.InitializeWaypoints();
        ai.ResetAI();
        ai.PickNewDestination();

        CustomerLife life = npc.GetComponent<CustomerLife>();
        if (life == null)
        {
            life = npc.AddComponent<CustomerLife>();
        }
        life.spawner = this;
    }
    public void CustomerReturned(GameObject npc)
    {
        pool.ReturnCustomer(npc);
        StartCoroutine(RespawnDelay(npc));
    }
    IEnumerator RespawnDelay(GameObject npc)
    {
        float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
        yield return new WaitForSeconds(delay);

        SpawnNPC();
    }
}

