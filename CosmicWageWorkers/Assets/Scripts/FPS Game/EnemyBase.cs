using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float health = 50f;


    public Material dmgMaterial;
    public Material currentMaterial;
    private Material[] originalMaterials;
    private Renderer enemyRenderer;
    private SkinnedMeshRenderer newEnemyRenderer;
    protected WaveSpawner spawner;
    Renderer[] renderers;

    public void Awake()
    {
        enemyRenderer = GetComponent<Renderer>();

        currentMaterial = GetComponent<Material>();

        currentMaterial = enemyRenderer.material;

        //New stuff
        
        renderers = GetComponentsInChildren<Renderer>();
    }
    // Called when the enemy takes damage
    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        StartCoroutine(DamageFlash());



        if (health <= 0f)
        {
            //enemyRenderer.material = dmgMaterial;
            Die();
        }
    }
    public void SetSpawner(WaveSpawner s)
    {
        spawner = s;
    }


    // Handles enemy death
    protected virtual void Die()
    {
        // Register a kill with the FPSManager
        if (FPSManager.Instance != null)
        {
            FPSManager.Instance.RegisterKill();
        }
        if (spawner != null)
        {
            spawner.OnEnemyKilled();
        }


        // Destroy this enemy object
        Destroy(gameObject);
    }

    IEnumerator DamageFlash()
    {
        float duration = 0.2f;
        float time = 0;

        while (time < duration)
        {
            float t = 1 - (time / duration);

            foreach (Renderer r in renderers)
            {
                foreach (Material m in r.materials)
                {
                    m.SetFloat("_FlashAmount", t);
                }
            }

            time += Time.deltaTime;
            yield return null;
        }

        foreach (Renderer r in renderers)
        {
            foreach (Material m in r.materials)
            {
                m.SetFloat("_FlashAmount", 0f);
            }
        }
    }
}