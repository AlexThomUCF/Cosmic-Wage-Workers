using UnityEngine;

public class PickupObject : MonoBehaviour
{
    PlayerFreeze freeze;

    public void Awake()
    {
        freeze = FindAnyObjectByType<PlayerFreeze>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
           freeze.ClearFreeze();
            Destroy(this.gameObject);
        }
    }
}
