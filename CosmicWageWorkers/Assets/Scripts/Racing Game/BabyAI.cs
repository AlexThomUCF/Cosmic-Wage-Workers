using UnityEngine;
using UnityEngine.AI;

public class BabyAI : MonoBehaviour
{
    public GameObject[] drivingWaypoints;
    private int currentWaypointIndex = 0;
    public NavMeshAgent babyAgent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        babyAgent = GetComponent<NavMeshAgent>();
        MoveToPoint();
    }

    // Update is called once per frame
    void Update()
    {
        if (!babyAgent.pathPending && babyAgent.remainingDistance < 1f)
        {
            MoveToPoint();
        }
    }

    public void MoveToPoint()
    {
        if (drivingWaypoints.Length == 0)
            return;

        babyAgent.SetDestination(drivingWaypoints[currentWaypointIndex].transform.position);

        // Increment index and wrap around
        currentWaypointIndex = (currentWaypointIndex + 1) % drivingWaypoints.Length;


    }
}
