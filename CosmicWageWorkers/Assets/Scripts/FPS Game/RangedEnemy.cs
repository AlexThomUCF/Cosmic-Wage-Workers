using UnityEngine;
using UnityEngine.AI;

public class RangedEnemy : EnemyBase
{
    [Header("References")]
    private Transform player;
    public NavMeshAgent agent;
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public Animator animator;

    [Header("Movement")]
    public float stoppingDistance = 2f; // Optional: keep minimum distance
    public float moveRange = 3f;

    [Header("Shooting")]
    public float projectileSpeed = 30f;
    public float shootCooldown = 2f;
    private float nextShootTime;


    private float shootTimer;

    private bool isNotHidden;

    public GUN gun;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            player = playerObj.transform;

        agent = GetComponent<NavMeshAgent>();
        gun = FindAnyObjectByType<GUN>();
        animator = GetComponent<Animator>();
        shootTimer = shootCooldown;

        isNotHidden = false;
    }

    void Update()
    {

        if (player == null) return;

        // Always move toward the player
        float distance = Vector3.Distance(transform.position, player.position);
        //Mathf.Round(distance);
        if (distance > agent.stoppingDistance)
        {
            agent.SetDestination(player.position);
            animator.SetBool("RifleIdle", false);
        }
        else if (distance < agent.stoppingDistance - moveRange)//Alex note(If distance < stopping distance, Position where player is not hidden and stops a distance away from them?)
        { 
            agent.SetDestination(-player.position);
            animator.SetBool("RifleIdle", true);
          //  Debug.Log(distance);
            
        }

            // Rotate toward player
            Vector3 lookDirection = player.position - transform.position;
        lookDirection.y = 0;
        if (lookDirection != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(lookDirection);

        BehindObject();// checks if player is behind an object


        // Handle shooting continuously
        if (Time.time >= nextShootTime)
        {
            nextShootTime = Time.time + shootCooldown;
            ShootAtPlayer();
        }
    }

    private void ShootAtPlayer()
    {
        if (projectilePrefab != null && shootPoint != null && !isNotHidden)
        {
            GameObject proj = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
            Projectile projectileScript = proj.GetComponent<Projectile>();
            if (projectileScript != null)
            {
                projectileScript.shooter = transform; // store who fired it
            }
            //Spawn particle at shootpoint.position
            //play SFXs
            Rigidbody rb = proj.GetComponent<Rigidbody>();

            Vector3 target = player.position + Vector3.up * 1.0f;

            rb.linearVelocity = (target - shootPoint.position).normalized * projectileSpeed;

            Destroy(proj, 5f);
        }
    }
    void BehindObject()
    {
        RaycastHit hit;

        Vector3 dir = (player.position - shootPoint.position).normalized;

        if (Physics.Raycast(shootPoint.position, dir, out hit, 100f))
        {
            if (hit.transform == player)
            {
                isNotHidden = false;
                //Debug.Log("Player not hidden");
            }
            else
            {
                isNotHidden = true;
                //Debug.Log("Player is hidden");
            }
        }
    }

}

