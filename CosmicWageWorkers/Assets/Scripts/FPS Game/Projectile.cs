using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 5f;
    public string targetTag = "Player"; // Enemy projectiles hit the player

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Freeze player if hit
        if (collision.collider.CompareTag(targetTag))
        {
            PlayerFreeze player = collision.collider.GetComponent<PlayerFreeze>();
            if (player != null)
            {
                player.FreezeHit();
            }
        }

        Destroy(gameObject);
    }
}