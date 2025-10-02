using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damage = 10f;
    public float lifetime = 5f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        EnemyBase enemy = collision.collider.GetComponent<EnemyBase>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        // Destroy projectile after hit
        Destroy(gameObject);
    }
}

