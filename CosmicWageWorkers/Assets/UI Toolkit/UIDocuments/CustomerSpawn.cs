using UnityEngine;

public class CustomerSpawn : MonoBehaviour
{
    public GameObject customer;

    public float spawnDelay;

    public float spawnRate;

    public Vector3 spawnPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InvokeRepeating("SpawnCustomer", spawnDelay, spawnRate);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnCustomer()
    {
        Instantiate(customer, spawnPos, Quaternion.identity);
    }
}
