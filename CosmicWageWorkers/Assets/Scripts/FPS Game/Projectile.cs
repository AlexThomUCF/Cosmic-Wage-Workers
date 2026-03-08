using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifetime = 5f;
    public string targetTag = "Player"; // Enemy projectiles hit the player

    public AudioSource audioSource;
    public AudioClip hitSound;



    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void OnCollisionEnter(Collision collision)
    {
        // Freeze player if hit
        if (collision.collider.CompareTag(targetTag))
        {
            PlayerFreeze player = collision.collider.GetComponent<PlayerFreeze>();
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
