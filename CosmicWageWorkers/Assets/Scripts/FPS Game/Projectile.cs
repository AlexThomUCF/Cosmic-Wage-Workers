using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Transform shooter;
    public float lifetime = 5f;
    public string targetTag = "Player"; // Enemy projectiles hit the player
    public bool isParried = false;
    public int damage = 25;

    public AudioSource audioSource;
    public AudioClip hitSound;



    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)

    {
        // If parried, ignore normal collision behavior
        if (isParried)
        {
            if (collision.collider.CompareTag("Enemy"))
            {
                EnemyBase enemy = collision.collider.GetComponent<EnemyBase>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }

                Destroy(gameObject);
            }

            return; // IMPORTANT: stop here
        }

        // Freeze player if hit
        if (collision.collider.CompareTag(targetTag))
        {
            PlayerFreeze player = collision.collider.GetComponent<PlayerFreeze>();
            EnemyBase enemy = collision.collider.GetComponent<EnemyBase>();
            if (player != null)
            {
                player.FreezeHit();
                AudioSource.PlayClipAtPoint(hitSound, transform.position);
                SoundEffectManager.Play("Freeze");
                Destroy(gameObject);

                slimehitEffect.Instance.PlayEffect();
            }
        }

        // Destroy projectile immediately on impact
        
        Destroy(gameObject);
        
    }
}
