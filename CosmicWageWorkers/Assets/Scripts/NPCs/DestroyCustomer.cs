using UnityEngine;

public class DestroyCustomer : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.GetComponent<CustomerAI>() != null)
        {
            Destroy(other.gameObject);
        }
    }
}
