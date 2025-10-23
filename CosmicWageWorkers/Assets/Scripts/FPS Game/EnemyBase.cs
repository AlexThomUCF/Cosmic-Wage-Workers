using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float health = 50f;

    // Called when the enemy takes damage
    public virtual void TakeDamage(float amount)
    {
        health -= amount;

        if (health <= 0f)
        {
            Die();
        }
    }

    // Handles enemy death
    protected virtual void Die()
    {
        // Register a kill with the FPSManager
        if (FPSManager.Instance != null)
        {
            FPSManager.Instance.RegisterKill();
        }

        // Destroy this enemy object
        Destroy(gameObject);
    }
}