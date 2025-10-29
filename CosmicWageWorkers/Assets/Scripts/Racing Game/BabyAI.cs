using UnityEngine;
using UnityEngine.AI;

public class BabyAI : MonoBehaviour
{
    public GameObject[] drivingWaypoints;
    public NavMeshAgent babyAgent;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        babyAgent = GetComponent<NavMeshAgent>();
        moveToPoint();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void moveToPoint()
    {
        if (babyAgent != null)
        {
            for (int i = 0; i < drivingWaypoints.Length; i++)
            {
                babyAgent.SetDestination(drivingWaypoints[i].transform.position);
            }
        }
    }
}
