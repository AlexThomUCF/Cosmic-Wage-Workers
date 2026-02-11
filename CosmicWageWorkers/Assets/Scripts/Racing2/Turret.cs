using UnityEngine;

public class TurretShootWhenSeePlayer : MonoBehaviour
{
    [Header("Target")]
    public Transform player;

    [Header("Vision")]
    public float viewDistance = 25f;
    public LayerMask obstacleMask;

    [Header("Shooting")]
    public Transform firePoint;
    public GameObject projectilePrefab;
    public float fireRate = 0.33f;
    public float projectileSpeed = 20f;
    public float spawnOffset = 0.1f;

    private float nextFireTime;

    void Update()
    {
        if (player == null || projectilePrefab == null || firePoint == null) return;

        if (CanSeePlayer() && Time.time >= nextFireTime)
        {
            Shoot();
            nextFireTime = Time.time + (1f / Mathf.Max(0.01f, fireRate));
        }
    }

    bool CanSeePlayer()
    {
        Vector3 toPlayer = player.position - firePoint.position;
        float dist = toPlayer.magnitude;
        if (dist > viewDistance) return false;

        Vector3 dir = toPlayer.normalized;

        // raycast detect
        if (Physics.Raycast(firePoint.position, dir, dist, obstacleMask))
            return false;

        return true;
    }

    void Shoot()
    {
        Vector3 dir = (player.position - firePoint.position).normalized;

        Vector3 spawnPos = firePoint.position + dir * spawnOffset;
        GameObject proj = Instantiate(projectilePrefab, spawnPos, Quaternion.LookRotation(dir));

        Rigidbody rb = proj.GetComponent<Rigidbody>();
        if (rb != null)
        {
            
            rb.linearVelocity = dir * projectileSpeed;
        }
        else
        {
            proj.transform.position += dir * 0.01f;
        }

        Destroy(proj, 5f);
    }

    void OnDrawGizmosSelected()
    {
        if (firePoint == null) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(firePoint.position, viewDistance);
    }
}
