using UnityEngine;
using UnityEngine.AI;

public class PatrolAndFlee : MonoBehaviour
{
    public Transform player;
    public Transform[] patrolPoints;

    public float fleeDistance = 5f;        // Check distance
    public float pointReachDist = 0.8f;    // Target distance
    public float repickTime = 0.5f;        // Pick destination

    private NavMeshAgent agent;
    private int patrolIndex = 0;
    private float timer;
    public class SnapToNavMesh : MonoBehaviour
    {
        void Start()
        {
            if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 2f, NavMesh.AllAreas)) //force nav on ground
            {
                transform.position = hit.position;
            }
        }
    }
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (patrolPoints != null && patrolPoints.Length > 0)
            agent.SetDestination(patrolPoints[patrolIndex].position);
    }

    void Update()
    {
        if (!player || patrolPoints == null || patrolPoints.Length == 0) return;

        float d = Vector3.Distance(transform.position, player.position);

        // 1) RUN when close
        if (d < fleeDistance)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                timer = repickTime;
                Transform far = GetFarthestPointFromPlayer();
                agent.SetDestination(far.position);
            }
            return;
        }

        // 2) Patrol through point
        if (!agent.pathPending && agent.remainingDistance <= pointReachDist)
        {
            patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
            agent.SetDestination(patrolPoints[patrolIndex].position);
        }
    }

    Transform GetFarthestPointFromPlayer()
    {
        Transform best = patrolPoints[0];
        float bestDist = -1f;

        for (int i = 0; i < patrolPoints.Length; i++)
        {
            float dist = Vector3.Distance(patrolPoints[i].position, player.position);
            if (dist > bestDist)
            {
                bestDist = dist;
                best = patrolPoints[i];
            }
        }
        return best;
    }
}
