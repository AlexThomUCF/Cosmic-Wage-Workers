using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]
    public float speed = 25f;          // How fast the bullet moves
    public float damage = 20f;         // How much damage it deals
    public float lifetime = 5f;        // Destroy after this time
    public string targetTag = "Enemy"; // Only hit enemies

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = transform.forward * speed;
        }

        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Only hit objects with the target tag
        if (collision.collider.CompareTag(targetTag))
        {
            EnemyBase enemy = collision.collider.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }

        // Destroy the projectile on impact
        Destroy(gameObject);
    }
}

