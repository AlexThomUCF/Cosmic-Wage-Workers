using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class CustomerAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform[] waypoints;

    private Transform currentTarget;
    private bool isWaiting;
    private float waitTime;
    private float waitCounter;
    private Coroutine rotateRoutine;

    void Start()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        PickNewDestination(); // Choose first random waypoint
    }

    void Update()
    {
        // Check if at waypoint and start waiting
        if (!isWaiting && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            isWaiting = true;
            waitTime = Random.Range(2f, 5f); // Pause at waypoint
            waitCounter = 0f;

            // Smoothly rotate toward the shelf
            if (rotateRoutine != null) StopCoroutine(rotateRoutine);
            rotateRoutine = StartCoroutine(SmoothFaceTarget(currentTarget));
        }
        
        // Waiting time
        if (isWaiting)
        {
            waitCounter += Time.deltaTime;
            if (waitCounter >= waitTime)
            {
                isWaiting = false;
                PickNewDestination(); // Move to next waypoint
            }
        }
    }

    // Select a random waypoint and set as agent's destination
    void PickNewDestination()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        currentTarget = waypoints[Random.Range(0, waypoints.Length)];
        agent.SetDestination(currentTarget.position);
    }

    // Smoothly rotate toward the waypoint's Z axis
    IEnumerator SmoothFaceTarget(Transform target)
    {
        if (target == null) yield break;

        Vector3 lookDir = target.forward;
        lookDir.y = 0; // Keep upright
        if (lookDir == Vector3.zero) yield break;

        Quaternion targetRot = Quaternion.LookRotation(lookDir);
        Quaternion startRot = transform.rotation;

        float duration = 0.5f;
        float t = 0f;

        while (t < duration)
        {
            transform.rotation = Quaternion.Slerp(startRot, targetRot, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRot; // Ensures the customer reached the target rotation
    }
}