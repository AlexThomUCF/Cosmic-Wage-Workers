using System.Collections.Generic;
using UnityEngine;

public class CustomerPool : MonoBehaviour
{
    public GameObject npcPrefab;
    public int poolSize = 10;

    private Queue<GameObject> pool = new Queue<GameObject>();

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject npc = Instantiate(npcPrefab, Vector3.zero, Quaternion.identity);

            // Attach CustomerLife if missing
            if (npc.GetComponent<CustomerLife>() == null)
            {
                CustomerLife life = npc.AddComponent<CustomerLife>();
                life.spawner = this.GetComponent<CustomerSpawning>();
            }

            npc.SetActive(false);
            pool.Enqueue(npc);
        }
    }

    public GameObject GetCustomer()
    {
        GameObject npc;

        if (pool.Count > 0)
        {
            npc = pool.Dequeue();
            npc.SetActive(true);
        }
        else
        {
            npc = Instantiate(npcPrefab, Vector3.zero, Quaternion.identity);
        }

        return npc;
    }

    public void ReturnCustomer(GameObject npc)
    {
        npc.SetActive(false);
        pool.Enqueue(npc);
    }
}