using UnityEngine;
using UnityEngine.AI;

public class PropAi : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform exit; // assigned by spawner

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Start()
    {
        if (exit == null)
        {
            Debug.LogError($"[PropAI] Exit not assigned on {name}");
            return;
        }

        agent.SetDestination(exit.position);
    }
}
