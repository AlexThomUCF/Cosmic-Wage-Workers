using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class SmartAIAvoidPlayer : MonoBehaviour
{
    public Transform player;
    private NavMeshAgent agent;

    [Header("Distances")]
    public float avoidDistance = 6f;
    public float safeDistance = 8f;
    public float fleeDistance = 10f;

    [Header("Movement Settings")]
    public float updateRate = 0.2f;
    private float timer = 0f;

    [Header("Flee Settings")]
    public int fleeAttempts = 12;
    public float maxHeightDifference = 2f;
    public float wallAvoidDistance = 3f;

    private bool isFleeing = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance < avoidDistance)
            isFleeing = true;
        else if (distance > safeDistance)
            isFleeing = false;

        if (isFleeing)
            FleeFromPlayer();
        else if (!agent.pathPending && agent.remainingDistance < 0.1f)
            agent.ResetPath();
    }

    void FleeFromPlayer()
    {
        timer += Time.deltaTime;
        if (timer < updateRate) return;
        timer = 0f;

        Vector3 bestTarget = transform.position;
        float bestScore = -Mathf.Infinity;

        Vector3 awayFromPlayer = (transform.position - player.position).normalized;

        for (int i = 0; i < fleeAttempts; i++)
        {
            Vector3 dir = awayFromPlayer;

            // Add randomness
            dir += Random.insideUnitSphere * 0.5f;
            dir = Vector3.ProjectOnPlane(dir, Vector3.up).normalized;

            // ?? Prevent moving toward the player
            if (Vector3.Dot(dir, awayFromPlayer) < 0.4f)
                continue;

            Vector3 testPos = transform.position + dir * fleeDistance;

            NavMeshHit hit;
            if (NavMesh.SamplePosition(testPos, out hit, fleeDistance + 5f, NavMesh.AllAreas))
            {
                // Height check
                if (Mathf.Abs(hit.position.y - transform.position.y) > maxHeightDifference)
                    continue;

                // Path validation
                NavMeshPath path = new NavMeshPath();
                if (!agent.CalculatePath(hit.position, path) || path.status != NavMeshPathStatus.PathComplete)
                    continue;

                // Wall avoidance penalty
                float wallPenalty = 0f;
                RaycastHit wallHit;

                if (Physics.Raycast(hit.position, Vector3.forward, out wallHit, wallAvoidDistance))
                    wallPenalty += wallAvoidDistance - wallHit.distance;
                if (Physics.Raycast(hit.position, Vector3.back, out wallHit, wallAvoidDistance))
                    wallPenalty += wallAvoidDistance - wallHit.distance;
                if (Physics.Raycast(hit.position, Vector3.right, out wallHit, wallAvoidDistance))
                    wallPenalty += wallAvoidDistance - wallHit.distance;
                if (Physics.Raycast(hit.position, Vector3.left, out wallHit, wallAvoidDistance))
                    wallPenalty += wallAvoidDistance - wallHit.distance;

                float distanceScore = Vector3.Distance(hit.position, player.position);

                float score = distanceScore - wallPenalty;

                if (score > bestScore)
                {
                    bestScore = score;
                    bestTarget = hit.position;
                }
            }
        }

        // Fallback if no good option found
        if (bestScore < 0f)
        {
            NavMeshHit fallback;
            if (NavMesh.SamplePosition(transform.position + Random.insideUnitSphere * fleeDistance, out fallback, fleeDistance, NavMesh.AllAreas))
                bestTarget = fallback.position;
        }

        agent.SetDestination(bestTarget);
    }

    // ?? Gizmos
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, avoidDistance);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, safeDistance);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, fleeDistance);
    }
}