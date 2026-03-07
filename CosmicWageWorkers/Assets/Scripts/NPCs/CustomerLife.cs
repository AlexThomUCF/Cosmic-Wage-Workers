using UnityEngine;

public class CustomerLife : MonoBehaviour
{
    public CustomerSpawning spawner;

    public void Start()
    {
        spawner = FindFirstObjectByType<CustomerSpawning>();
    }
    public void RemoveCustomer()
    {
        spawner.CustomerReturned(gameObject);
    }
}