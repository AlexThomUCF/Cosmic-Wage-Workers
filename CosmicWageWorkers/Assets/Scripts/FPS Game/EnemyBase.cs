using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public float health = 1f;

    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0f)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
