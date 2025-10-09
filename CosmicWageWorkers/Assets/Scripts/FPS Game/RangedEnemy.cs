using UnityEngine;
using UnityEngine.AI;

public class RangedEnemy : EnemyBase
{
    [Header("Movement")]
    public float stoppingDistance = 2f; // Optional: keep minimum distance

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
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        agent = GetComponent<NavMeshAgent>();
        shootTimer = shootCooldown;
    }

    void Update()
    {
        if (player == null) return;

        // Always move toward the player
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance > stoppingDistance)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            agent.ResetPath(); // Optional: stop very close to player
        }

        // Rotate toward player
        Vector3 lookDirection = player.position - transform.position;
        lookDirection.y = 0;
        if (lookDirection != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(lookDirection);

        // Handle shooting continuously
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            shootTimer = shootCooldown;
            ShootAtPlayer();
        }
    }

    private void ShootAtPlayer()
    {
        if (projectilePrefab != null && shootPoint != null)
        {
            GameObject proj = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
            Rigidbody rb = proj.GetComponent<Rigidbody>();
            rb.linearVelocity = (player.position - shootPoint.position).normalized * projectileSpeed;

            Destroy(proj, 5f);
        }
    }
}

