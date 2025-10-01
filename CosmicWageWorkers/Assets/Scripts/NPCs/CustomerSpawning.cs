using UnityEngine;

public class CustomerSpawning : MonoBehaviour
{
    public GameObject customerPrefab;
    void Start()
    {
        Instantiate(customerPrefab, transform.position, Quaternion.identity);
    }
}
