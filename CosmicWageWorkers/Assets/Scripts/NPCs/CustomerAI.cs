using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class CustomerAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform[] waypoints;
    public Transform[] finalWaypoints; // The specific waypoint to go after visiting N waypoints
    public Transform exitWaypoint;
    public int maxVisits = 3;       // Number of random waypoints before going to finalWaypoint

    private Transform currentTarget;
    private bool isWaiting;
    private float waitTime;
    private float waitCounter;
    private Coroutine rotateRoutine;
    private Animator animator;

    private int visitedCount = 0; // Track how many waypoints the NPC has visited

    // SHARED across all customers
    private static Dictionary<Transform, bool> occupiedWaypoints = new Dictionary<Transform, bool>();

    void Start()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (animator == null) animator = GetComponent<Animator>();

        // Small QoL tweaks to reduce pushing
        agent.stoppingDistance = 0.8f;
        agent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        agent.avoidancePriority = Random.Range(30, 60);

        // Initialize shared waypoint dictionary safely
        if (waypoints != null)
        {
            foreach (var wp in waypoints)
            {
                if (!occupiedWaypoints.ContainsKey(wp))
                    occupiedWaypoints.Add(wp, false);
            }
        }

        // Initialize final waypoints
        if (finalWaypoints != null)
        {
            foreach (var wp in finalWaypoints)
            {
                if (!occupiedWaypoints.ContainsKey(wp))
                    occupiedWaypoints.Add(wp, false);
            }
        }

        PickNewDestination();
    }

    void Update()
    {
        // Check if agent has reached its destination
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            // Only trigger this once per waypoint
            if (!isWaiting)
            {
                isWaiting = true;
                animator.SetTrigger("IsWaiting");

                waitTime = Random.Range(2f, 5f);
                waitCounter = 0f;

                if (rotateRoutine != null)
                    StopCoroutine(rotateRoutine);

                rotateRoutine = StartCoroutine(SmoothFaceTarget(currentTarget));
            }
        }

        if (isWaiting)
        {
            waitCounter += Time.deltaTime;

            if (waitCounter >= waitTime)
            {
                isWaiting = false;
                animator.SetTrigger("IsWalking");

                // Free the waypoint
                if (currentTarget != null && occupiedWaypoints.ContainsKey(currentTarget))
                    occupiedWaypoints[currentTarget] = false;

                // If this was a regular waypoint, increment visit count
                if (currentTarget != null && (finalWaypoints == null || !System.Array.Exists(finalWaypoints, f => f == currentTarget)))
                {
                    visitedCount++;
                    PickNewDestination(); // go to next waypoint
                }
                else if (System.Array.Exists(finalWaypoints, f => f == currentTarget))
                {
                    // Only start leaving AFTER reaching final waypoint
                    StartCoroutine(LeaveAfterWait());
                }
            }
        }
    }

    void PickNewDestination()
    {
        // If visited enough, pick a final waypoint
        if (visitedCount >= maxVisits && finalWaypoints != null && finalWaypoints.Length > 0)
        {
            // Shuffle or pick randomly among the final waypoints
            List<Transform> availableFinals = new List<Transform>();
            foreach (var wp in finalWaypoints)
            {
                if (!occupiedWaypoints[wp])
                    availableFinals.Add(wp);
            }

            if (availableFinals.Count > 0)
            {
                currentTarget = availableFinals[Random.Range(0, availableFinals.Count)];
                occupiedWaypoints[currentTarget] = true;
                agent.SetDestination(currentTarget.position);
                return; // done, moving to final waypoint
            }
            else
            {
                // All final waypoints are busy → pick a random normal waypoint
                visitedCount--; // decrement so we keep trying for final later
            }

        }


        // Otherwise, pick a random available waypoint

        List<Transform> availableWaypoints = new List<Transform>();

        foreach (var wp in waypoints)
        {
            if (occupiedWaypoints.ContainsKey(wp) && !occupiedWaypoints[wp])
                availableWaypoints.Add(wp);
        }

        if (availableWaypoints.Count == 0)
            return; // All occupied — try again later

        currentTarget = availableWaypoints[Random.Range(0, availableWaypoints.Count)];
        occupiedWaypoints[currentTarget] = true;

        agent.SetDestination(currentTarget.position);
    }

    IEnumerator SmoothFaceTarget(Transform target)
    {
        if (target == null)
            yield break;

        Vector3 lookDir = target.forward;
        lookDir.y = 0f;

        if (lookDir == Vector3.zero)
            yield break;

        Quaternion startRot = transform.rotation;
        Quaternion targetRot = Quaternion.LookRotation(lookDir);

        float duration = 0.5f;
        float t = 0f;

        while (t < duration)
        {
            transform.rotation = Quaternion.Slerp(startRot, targetRot, t / duration);
            t += Time.deltaTime;
            yield return null;
        }

        transform.rotation = targetRot;
    }

    IEnumerator LeaveAfterWait()
    {
        // Wait a moment at the final waypoint
        yield return new WaitForSeconds(1f);

        // Free the final waypoint
        if (currentTarget != null && occupiedWaypoints.ContainsKey(currentTarget))
            occupiedWaypoints[currentTarget] = false;


        Debug.Log("YOU CAN DESTROY ME");
        agent.SetDestination(exitWaypoint.position);

      
    }

    void OnDestroy()
    {
        // Safety cleanup in case an NPC despawns mid-walk
        if (currentTarget != null && occupiedWaypoints.ContainsKey(currentTarget))
            occupiedWaypoints[currentTarget] = false;
    }
}