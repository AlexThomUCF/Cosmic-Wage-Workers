using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NavMeshHopper : MonoBehaviour
{
    public float hopHeight = 1.5f;
    public float hopDistance = 1.2f;
    public float hopSpeed = 4f;

    private NavMeshAgent agent;
    private Vector3 startPos;
    private Vector3 endPos;
    private float hopProgress = 1f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        agent.updatePosition = false;
        agent.updateRotation = true;
    }

    void Update()
    {
        // Sync agent to actual position
        agent.nextPosition = transform.position;

        // Continue hop
        if (hopProgress < 1f)
        {
            hopProgress += Time.deltaTime * hopSpeed;

            float height = Mathf.Sin(hopProgress * Mathf.PI) * hopHeight;
            Vector3 pos = Vector3.Lerp(startPos, endPos, hopProgress);
            pos.y += height;

            transform.position = pos;
            return;
        }

        // Start new hop
        if (agent.hasPath && agent.remainingDistance > 0.05f)
        {
            Vector3 direction = agent.desiredVelocity.normalized;

            // ?? If direction is zero, don't hop
            if (direction.sqrMagnitude < 0.01f) return;

            float distance = Mathf.Min(hopDistance, agent.remainingDistance);

            startPos = transform.position;
            endPos = transform.position + direction * distance;

            hopProgress = 0f;
        }
    }
}