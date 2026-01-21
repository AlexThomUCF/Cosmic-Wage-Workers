using System.Collections;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public float health = 50f;

    private Renderer rend;
    private Color originalColor;

    private void Awake()
    {
        rend = GetComponentInChildren<Renderer>();
        originalColor = rend.material.color;
    }

    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        StartCoroutine(Flash());

        if (health <= 0f)
            Die();
    }

    private IEnumerator Flash()
    {
        rend.material.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        rend.material.color = originalColor;
    }

    protected virtual void Die()
    {
        FPSManager.Instance?.RegisterKill();
        Destroy(gameObject);
    }
}
