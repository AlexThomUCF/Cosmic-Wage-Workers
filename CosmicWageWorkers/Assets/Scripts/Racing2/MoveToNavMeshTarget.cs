using UnityEngine;
using UnityEngine.AI;

public class MoveToNavMeshTarget : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform target;

    void Update()
    {
        NavMeshHit hit;
        // Find nearest point on NavMesh within 1 unit
        if (NavMesh.SamplePosition(target.position, out hit, 1.0f, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
}