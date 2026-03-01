using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TeenAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public List<Transform> waypoints = new List<Transform>();

    [Header("Flee Settings")]
    public float runWhenDistanceLessThan = 25f;   // escape range
    public float repathInterval = 0.5f;          // waypoint cd
    public float minWaypointDistanceFromPlayer = 8f; // location select

    private NavMeshAgent agent;
    private float timer;
    private Transform currentTarget;

    public AudioSource attractsound;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (player == null || waypoints == null || waypoints.Count == 0) return;

        float distToPlayer = Vector3.Distance(transform.position, player.position);//detect range from player 

        if (distToPlayer > runWhenDistanceLessThan) return; //detect if too close to player

        timer -= Time.deltaTime;

        //stop when close enough to target
        if (currentTarget != null && Vector3.Distance(transform.position, currentTarget.position) > 1.2f)
        {
            
            if (timer > 0f) return;
            attractsound.Play();
        }

        if (timer > 0f) return;
        timer = repathInterval;

        currentTarget = PickBestWaypoint();

        if (currentTarget != null)
            agent.SetDestination(currentTarget.position);
    }

  Transform PickBestWaypoint()
{
    Transform best = null;
    float bestDist = -1f;

    Vector3 awayDir = (transform.position - player.position).normalized;
    float minDist = minWaypointDistanceFromPlayer;

    foreach (Transform wp in waypoints)
    {
        if (wp == null) continue;

        // avoid toward player
        Vector3 toWpDir = (wp.position - transform.position).normalized;
        if (Vector3.Dot(awayDir, toWpDir) <= 0f) continue;

        // target to player distance
        float distToPlayer = Vector3.Distance(wp.position, player.position);
        if (distToPlayer < minDist) continue;

        // pick the furthest
        if (distToPlayer > bestDist)
        {
            bestDist = distToPlayer;
            best = wp;
        }
    }

    // pick furthest in none match
    if (best == null)
    {
        foreach (Transform wp in waypoints)
        {
            if (wp == null) continue;

            float distToPlayer = Vector3.Distance(wp.position, player.position);
            if (distToPlayer > bestDist)
            {
                bestDist = distToPlayer;
                best = wp;
            }
        }
    }

    return best;
}


}
