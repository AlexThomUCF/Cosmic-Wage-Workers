using UnityEngine;
using UnityEngine.AI;

public class TeenAI : MonoBehaviour
{
    [Header("References")]
    public Transform player;

    [Header("Flee Settings")]
    public float runWhenDistanceLessThan = 25f;
    public float fleeDistance = 12f;
    public float repathInterval = 0.2f;
    public float rotateSpeed = 12f;
    public float destinationReachThreshold = 1.0f;

    [Header("Movement Options")]
    public bool alwaysFlee = false;
    public bool faceMovementDirection = true;

    private NavMeshAgent agent;
    private float repathTimer;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();

        if (agent != null)
        {
            agent.updateRotation = false;
            agent.autoBraking = false;
            agent.stoppingDistance = 0f;
        }
    }

    void Update()
    {
        if (player == null) return;
        if (agent == null || !agent.isOnNavMesh) return;

        bool shouldFlee = alwaysFlee ||
                          Vector3.Distance(transform.position, player.position) <= runWhenDistanceLessThan;

        if (!shouldFlee)
        {
            return;
        }

        repathTimer -= Time.deltaTime;

        bool needsNewPath =
            !agent.hasPath ||
            agent.pathPending == false && agent.remainingDistance <= destinationReachThreshold ||
            agent.pathStatus != NavMeshPathStatus.PathComplete ||
            repathTimer <= 0f;

        if (needsNewPath)
        {
            repathTimer = repathInterval;
            SetBestFleeDestination();
        }

        UpdateFacing();
    }

    void SetBestFleeDestination()
    {
        Vector3 away = transform.position - player.position;
        away.y = 0f;

        if (away.sqrMagnitude < 0.001f)
            away = transform.forward;
        else
            away.Normalize();

        Vector3 right = Vector3.Cross(Vector3.up, away).normalized;

        Vector3[] candidateDirs =
        {
            away,
            (away + right * 0.5f).normalized,
            (away - right * 0.5f).normalized,
            (away + right).normalized,
            (away - right).normalized
        };

        Vector3 bestPoint = transform.position;
        float bestScore = float.NegativeInfinity;
        bool found = false;

        for (int i = 0; i < candidateDirs.Length; i++)
        {
            Vector3 candidate = transform.position + candidateDirs[i] * fleeDistance;

            NavMeshHit hit;
            if (!NavMesh.SamplePosition(candidate, out hit, 3f, NavMesh.AllAreas))
                continue;

            NavMeshPath path = new NavMeshPath();
            if (!agent.CalculatePath(hit.position, path))
                continue;

            if (path.status != NavMeshPathStatus.PathComplete)
                continue;

            float distFromPlayer = Vector3.Distance(hit.position, player.position);
            float distFromSelf = Vector3.Distance(hit.position, transform.position);

            // prefer points farther from player, but still actually move somewhere
            float score = distFromPlayer + distFromSelf * 0.25f;

            if (score > bestScore)
            {
                bestScore = score;
                bestPoint = hit.position;
                found = true;
            }
        }

        if (found)
        {
            agent.isStopped = false;
            agent.SetDestination(bestPoint);
        }
    }

    void UpdateFacing()
    {
        Vector3 lookDir = Vector3.zero;

        if (faceMovementDirection)
        {
            lookDir = agent.desiredVelocity;
            lookDir.y = 0f;
        }

        if (lookDir.sqrMagnitude < 0.01f)
        {
            lookDir = transform.position - player.position;
            lookDir.y = 0f;
        }

        if (lookDir.sqrMagnitude < 0.001f) return;

        Quaternion targetRot = Quaternion.LookRotation(lookDir.normalized);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            targetRot,
            rotateSpeed * Time.deltaTime
        );
    }
}