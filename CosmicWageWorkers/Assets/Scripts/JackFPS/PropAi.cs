using UnityEngine;
using UnityEngine.AI;

public class PropAi : MonoBehaviour
{
    private NavMeshAgent agent;
    private ExitManager exitManager;
    private Transform chosenExit;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        exitManager = FindObjectOfType<ExitManager>();
    }

    void Start()
    {
        if (exitManager == null) return;

        chosenExit = exitManager.GetRandomExit();

        if (chosenExit != null)
        {
            agent.SetDestination(chosenExit.position);
        }
    }
}
