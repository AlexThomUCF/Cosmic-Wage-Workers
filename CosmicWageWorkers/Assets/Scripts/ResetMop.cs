using UnityEngine;

public class ResetMop : MonoBehaviour
{
    public RespawnItem respawn;
    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Mop"))
        {
            respawn.Respawn();
        }
    }
}
