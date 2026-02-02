using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;

public class CustomerAI : MonoBehaviour
{
    public NavMeshAgent agent;
    public Transform[] waypoints;

    private Transform currentTarget;
    private bool isWaiting;
    private float waitTime;
    private float waitCounter;
    private Coroutine rotateRoutine;
    private Animator animator;

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

        PickNewDestination();
    }

    void Update()
    {
        if (!isWaiting && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            isWaiting = true;
            animator.SetTrigger("IsWaiting");

            waitTime = Random.Range(2f, 5f);
            waitCounter = 0f;

            if (rotateRoutine != null)
                StopCoroutine(rotateRoutine);

            rotateRoutine = StartCoroutine(SmoothFaceTarget(currentTarget));
        }

        if (isWaiting)
        {
            waitCounter += Time.deltaTime;

            if (waitCounter >= waitTime)
            {
                isWaiting = false;
                animator.SetTrigger("IsWalking");

                // Free the waypoint when leaving
                if (currentTarget != null && occupiedWaypoints.ContainsKey(currentTarget))
                    occupiedWaypoints[currentTarget] = false;

                PickNewDestination();
            }
        }
    }

    void PickNewDestination()
    {
        if (waypoints == null || waypoints.Length == 0)
            return;

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

    void OnDestroy()
    {
        // Safety cleanup in case an NPC despawns mid-walk
        if (currentTarget != null && occupiedWaypoints.ContainsKey(currentTarget))
            occupiedWaypoints[currentTarget] = false;
    }
}