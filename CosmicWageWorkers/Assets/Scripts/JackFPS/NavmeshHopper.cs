using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshHopper : MonoBehaviour
{
    public float hopHeight = 1.5f;
    public float hopDistance = 1.2f;
    public float hopSpeed = 4f;

    [Header("Grounding")]
    public float navSampleRadius = 2f;
    public float groundOffset = 0.05f; // keeps it slightly above ground

    private NavMeshAgent agent;
    private Vector3 startPos;
    private Vector3 endPos;
    private float hopProgress = 1f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.updatePosition = false;
        agent.updateRotation = false; // we'll handle rotation manually
    }

    void Update()
    {
        // Keep NavMeshAgent synced
        agent.nextPosition = transform.position;

        // === CONTINUE HOP ===
        if (hopProgress < 1f)
        {
            hopProgress += Time.deltaTime * hopSpeed;

            float height = Mathf.Sin(hopProgress * Mathf.PI) * hopHeight;
            Vector3 pos = Vector3.Lerp(startPos, endPos, hopProgress);

            // Snap to NavMesh height
            NavMeshHit hit;
            if (NavMesh.SamplePosition(pos, out hit, navSampleRadius, NavMesh.AllAreas))
            {
                pos.y = hit.position.y + height + groundOffset;
            }

            transform.position = pos;

            // Snap cleanly when hop ends
            if (hopProgress >= 1f)
            {
                if (NavMesh.SamplePosition(transform.position, out hit, navSampleRadius, NavMesh.AllAreas))
                {
                    transform.position = hit.position + Vector3.up * groundOffset;
                }
            }

            return;
        }

        // === START NEW HOP ===
        if (agent.hasPath && agent.remainingDistance > 0.2f)
        {
            // Better turning
            Vector3 direction = (agent.steeringTarget - transform.position).normalized;

            if (direction.sqrMagnitude < 0.01f) return;

            float distance = Mathf.Min(hopDistance, agent.remainingDistance);

            startPos = transform.position;
            endPos = transform.position + direction * distance;

            // Clamp end position to NavMesh
            NavMeshHit hit;
            if (NavMesh.SamplePosition(endPos, out hit, navSampleRadius, NavMesh.AllAreas))
            {
                endPos = hit.position;
            }

            hopProgress = 0f;

            // Smooth rotation
            Quaternion rot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Time.deltaTime * 10f);
        }
    }
}