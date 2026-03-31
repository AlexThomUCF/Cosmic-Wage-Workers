using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float health = 50f;

    [Header("Materials")]
    public Material dmgMaterial;
    public Material currentMaterial;
    private Material[] originalMaterials;

    [Header("References")]
    public BoxCollider rangedCollider;
    private Renderer enemyRenderer;
    private SkinnedMeshRenderer newEnemyRenderer;
    private Animator animator;
    private RangedEnemy rangedEnemy;
    protected WaveSpawner spawner;
    Renderer[] renderers;

    public void Awake()
    {
        enemyRenderer = GetComponent<Renderer>();

        currentMaterial = GetComponent<Material>();

        currentMaterial = enemyRenderer.material;

        //New stuff
        
        renderers = GetComponentsInChildren<Renderer>();

        animator = GetComponent<Animator>();

        rangedEnemy = GetComponent<RangedEnemy>();
    }
    // Called when the enemy takes damage
    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        StartCoroutine(DamageFlash());



        if (health <= 0f)
        {
            //enemyRenderer.material = dmgMaterial;
            if(animator != null && rangedEnemy != null)
            {
                animator.SetTrigger("Dead");
                rangedEnemy.agent.isStopped = true;
                rangedCollider.enabled = false;
                rangedEnemy.enabled = false; //Turns off movement script
            }
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
        Destroy(gameObject, 3f);
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