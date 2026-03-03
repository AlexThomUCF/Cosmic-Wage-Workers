using UnityEngine;

public class DestroyCustomer : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<CustomerAI>() != null)
        {
            Destroy(other.gameObject);
        }
    }
}
