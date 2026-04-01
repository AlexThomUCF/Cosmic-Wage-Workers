using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ParryLogic : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image parryImage;
    [SerializeField] private GameObject mop;
    [SerializeField] private Transform[] uiPositions;
    [SerializeField] private Transform[] mopPositions;
    [SerializeField] private Transform defaultMopPosition;
    [SerializeField] private Animator animator;

    [Header("Parry Settings")]
    public int bulletCounter = 0;
    public int maxBulletDeflect = 5;
    public bool resetParry = false;

    public bool isResetting = false;

    void Awake()
    {
        if (parryImage != null)
            parryImage.enabled = false;
    }

    public void ResetParryState()
    {
        bulletCounter = 0;
        resetParry = false;
        StartCoroutine(SmoothReset());
        // parryImage.enabled = false; // no longer used
    }

    public bool IsMaxed()
    {
        return bulletCounter >= maxBulletDeflect;
    }

    IEnumerator SmoothReset()
    {
        if (isResetting) yield break;
        if (mop == null || defaultMopPosition == null) yield break;

        isResetting = true;

        // Disable animator while moving
        if (animator != null)
            animator.enabled = false;

        Vector3 startPos = mop.transform.position;
        Quaternion startRot = mop.transform.rotation;

        float duration = 1f; // 1 second reset
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0, 1, elapsed / duration);

            mop.transform.position = Vector3.Lerp(startPos, defaultMopPosition.position, t);
            mop.transform.rotation = Quaternion.Lerp(startRot, defaultMopPosition.rotation, t);

            yield return null;
        }

        // Ensure final position
        mop.transform.position = defaultMopPosition.position;
        mop.transform.rotation = defaultMopPosition.rotation;

        // Reset counters
        bulletCounter = 0;
        resetParry = false;
        Debug.Log("it got here");

        // Wait one frame before re-enabling animator
        yield return null;
        if (animator != null)
            animator.enabled = true;

        isResetting = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EnemyBullet"))
        {
            Debug.Log("Bullet hit");

            if (bulletCounter < maxBulletDeflect && bulletCounter < mopPositions.Length)
            {
                // Disable animator for smooth movement
                if (animator != null) animator.enabled = false;

                // Move mop to current parry position
                mop.transform.position = mopPositions[bulletCounter].position;
                mop.transform.rotation = mopPositions[bulletCounter].rotation;

                // Handle bullet reflection
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

                bulletCounter++;

                // Trigger smooth reset if max reached
                if (bulletCounter >= maxBulletDeflect)
                {
                    StartCoroutine(SmoothReset());
                }
            }
        }
    }
}