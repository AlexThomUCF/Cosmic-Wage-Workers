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

    private Material[][] originalMaterialsPerRenderer;

    void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();

        originalMaterialsPerRenderer = new Material[renderers.Length][];

        for (int i = 0; i < renderers.Length; i++)
        {
            originalMaterialsPerRenderer[i] = renderers[i].materials;
        }

        animator = GetComponent<Animator>();
        rangedEnemy = GetComponent<RangedEnemy>();


    }
    // Called when the enemy takes damage
    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        StartCoroutine(DamageFlash());

        if(rangedEnemy != null && health > 0f)
        {
            SoundEffectManager.Play("HumanHit");
            animator.SetTrigger("Hit");
            
        }

        if (health <= 0f)
        {
            //enemyRenderer.material = dmgMaterial;
            if(animator != null && rangedEnemy != null)
            {
                animator.SetTrigger("Dead");
                SoundEffectManager.Play("DeathSound");
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
        if(rangedEnemy  != null)
        {
            Destroy(gameObject, 3f);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    IEnumerator DamageFlash()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            Material[] dmgMats = new Material[renderers[i].materials.Length];
            for (int j = 0; j < dmgMats.Length; j++)
            {
                dmgMats[j] = dmgMaterial;
            }
            renderers[i].materials = dmgMats;
        }

        yield return new WaitForSeconds(0.2f);

        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].materials = originalMaterialsPerRenderer[i];
        }
    }
}