using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float health = 50f;


    public Material dmgMaterial;
    public Material currentMaterial;
    private Renderer enemyRenderer;

    public void Awake()
    {
        enemyRenderer = GetComponent<Renderer>();

        currentMaterial = GetComponent<Material>();

        currentMaterial = enemyRenderer.material;
    }
    // Called when the enemy takes damage
    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        StartCoroutine(DamageFlash());



        if (health <= 0f)
        {
            enemyRenderer.material = dmgMaterial;
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

    IEnumerator DamageFlash()
    {
        enemyRenderer.material = dmgMaterial;
        yield return new WaitForSeconds(.2f);
        enemyRenderer.material = currentMaterial;

    }
}