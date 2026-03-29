using UnityEngine;
using UnityEngine.AI;

public class RangedEnemy : EnemyBase
{
    [Header("Movement")]
    public float stoppingDistance = 2f; // Optional: keep minimum distance
    public float moveRange = 3f;

    [Header("Shooting")]
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float projectileSpeed = 30f;
    public float shootCooldown = 2f;

    private Transform player;
    private NavMeshAgent agent;
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
        shootTimer = shootCooldown;
    }

    void Update()
    {

        isNotHidden = false;

        if (player == null) return;

        // Always move toward the player
        float distance = Vector3.Distance(transform.position, player.position);
        //Mathf.Round(distance);
        if (distance > agent.stoppingDistance)
        {
            agent.SetDestination(player.position);
        }
        else if (distance < agent.stoppingDistance - moveRange)//Alex note(If distance < stopping distance, Position where player is not hidden and stops a distance away from them?)
        { // When player gets to close to ranged enemy move them? or when player aims at enemy move them then have them wait a while to move again?
            //agent.SetDestination
            //agent.ResetPath(); // Optional: stop very close to player
           // Debug.Log("Too close to me");
            agent.SetDestination(-player.position);
          //  Debug.Log(distance);
            
        }

            // Rotate toward player
            Vector3 lookDirection = player.position - transform.position;
        lookDirection.y = 0;
        if (lookDirection != Vector3.zero)
            transform.rotation = Quaternion.LookRotation(lookDirection);

        BehindObject();// checks if player is behind an object

        
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
        if (projectilePrefab != null && shootPoint != null && isNotHidden)
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
            rb.linearVelocity = (player.position - shootPoint.position).normalized * projectileSpeed;

            Destroy(proj, 5f);
        }
    }
    void BehindObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, (player.position - transform.position), out hit, Mathf.Infinity))//if raycast from enemy to player 
        {
            if (hit.transform ==player) //if hit true
            {
                isNotHidden = true;
                //Debug.Log("Player not hidden");
            }
            else // if behind object
            {
                //Debug.Log("Player is Hidden");
            }
        }
        else
        {
            //Debug.Log("Hit nothing, can't find player");
        }
    }
   
}

