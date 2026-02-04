using UnityEngine;
using UnityEngine.AI;


public class PropAi : MonoBehaviour
{
    public NavMeshAgent agent;
    private GameObject exit;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;

        exit = GameObject.FindGameObjectWithTag("Exit");
        
    }
    void Start()
    {
        agent.SetDestination(exit.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
