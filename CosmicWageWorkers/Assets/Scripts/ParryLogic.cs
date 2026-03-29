using UnityEngine;
using UnityEngine.UI;
public class ParryLogic : MonoBehaviour
{
    public Image parryImage;
    public Transform[] uiPositions;
    public int bulletCounter = 0;
    public int maxBulletDeflect = 5;
    public bool resetParry = false;
    public void Awake()
    {
        parryImage.enabled = false;
    }

    public void ResetParryState()
    {
        bulletCounter = 0;
        resetParry = false;
        parryImage.enabled = false;
    }

    public bool IsMaxed()
    {
        return bulletCounter >= maxBulletDeflect;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("EnemyBullet"))
        {
            
            Debug.Log("Bullet hit");
            if (bulletCounter < maxBulletDeflect)
            {
                parryImage.enabled = true;

                // Move image to corresponding position
                parryImage.rectTransform.position = uiPositions[bulletCounter].position;
                parryImage.rectTransform.rotation = uiPositions[bulletCounter].rotation;

                Rigidbody rb = other.GetComponent<Rigidbody>();
                Projectile proj = other.GetComponent<Projectile>();

                if (proj != null && rb != null)
                {
                    proj.isParried = true;

                    if (proj.shooter != null)
                    {
                        Vector3 targetPos = proj.shooter.position + Vector3.up * 0.1f;
                        Vector3 dir = (targetPos - transform.position).normalized;

                        rb.linearVelocity = dir * rb.linearVelocity.magnitude;
                    }

                    proj.targetTag = "Enemy";
                }

                // OR rotate instead:


                bulletCounter++;
            }
            //each time a bullet hits parryImage needs to change rotation and trasforms
            //play sound effect
            //reflect bullet
           
        }
    }
}
