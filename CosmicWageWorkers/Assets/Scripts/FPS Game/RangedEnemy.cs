using UnityEngine;
using UnityEngine.AI;

public class RangedEnemy : EnemyBase
{
    [Header("Movement")]
    public float stoppingDistance = 10f;

    [Header("Shooting")]
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float projectileSpeed = 15f;
    public float shootCooldown = 2f;

    private Transform player;
    private NavMeshAgent agent;
    private float shootTimer;

    void Start()
    {
        // Find the player by tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }

        agent = GetComponent<NavMeshAgent>();
        shootTimer = shootCooldown;
    }

    void Update()
    {
        if (player == null) return;

        // Move toward player if too far
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > stoppingDistance)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            agent.ResetPath();
            TryShoot();
        }

        // Rotate enemy toward player (but ignore vertical)
        Vector3 lookDirection = player.position - transform.position;
        lookDirection.y = 0; // keep enemy upright
        if (lookDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }
    }

    void TryShoot()
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            shootTimer = shootCooldown;

            if (projectilePrefab != null && shootPoint != null)
            {
                GameObject proj = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
                Rigidbody rb = proj.GetComponent<Rigidbody>();
                rb.linearVelocity = (player.position - shootPoint.position).normalized * projectileSpeed;

                // Optional: destroy projectile after 5 seconds
                Destroy(proj, 5f);
            }
        }
    }
}
