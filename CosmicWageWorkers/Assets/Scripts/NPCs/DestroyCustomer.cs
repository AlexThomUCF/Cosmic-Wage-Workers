using UnityEngine;

public class DestroyCustomer : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        CustomerLife life = other.gameObject.GetComponent<CustomerLife>();

        if (life != null)
        {
            life.RemoveCustomer();
        }
    }
}