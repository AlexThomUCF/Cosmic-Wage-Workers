using UnityEngine;
using UnityEngine.AI;

public class RangedEnemy : EnemyBase
{
    public float stoppingDistance = 10f;
    public float shootCooldown = 2f;
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float projectileSpeed = 15f;

    private Transform player;
    private NavMeshAgent agent;
    private float shootTimer;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        // Move toward player if too far
        if (dist > stoppingDistance)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            agent.ResetPath();
            TryShoot();
        }

        transform.LookAt(player.position);
    }

    void TryShoot()
    {
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            shootTimer = shootCooldown;
            GameObject proj = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
            Rigidbody rb = proj.GetComponent<Rigidbody>();
            rb.linearVelocity = (player.position - shootPoint.position).normalized * projectileSpeed;
        }
    }
}

