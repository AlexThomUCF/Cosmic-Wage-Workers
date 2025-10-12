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

    // Static dictionary to track which waypoints are occupied
    private static Dictionary<Transform, bool> occupiedWaypoints = new Dictionary<Transform, bool>();

    void Start()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();

        // Initialize occupied dictionary if empty
        if (occupiedWaypoints.Count == 0 && waypoints != null)
        {
            foreach (var wp in waypoints)
                occupiedWaypoints[wp] = false;
        }

        PickNewDestination();
    }

    void Update()
    {
        if (!isWaiting && !agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            isWaiting = true;
            waitTime = Random.Range(2f, 5f);
            waitCounter = 0f;

            if (rotateRoutine != null) StopCoroutine(rotateRoutine);
            rotateRoutine = StartCoroutine(SmoothFaceTarget(currentTarget));
        }

        if (isWaiting)
        {
            waitCounter += Time.deltaTime;
            if (waitCounter >= waitTime)
            {
                isWaiting = false;

                // Mark current waypoint as free
                if (currentTarget != null)
                    occupiedWaypoints[currentTarget] = false;

                PickNewDestination();
            }
        }
    }

    void PickNewDestination()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        List<Transform> availableWaypoints = new List<Transform>();
        foreach (var wp in waypoints)
        {
            if (!occupiedWaypoints[wp])
                availableWaypoints.Add(wp);
        }

        if (availableWaypoints.Count == 0)
        {
            // All waypoints occupied, just wait and retry next frame
            return;
        }

        // Pick a random free waypoint
        currentTarget = availableWaypoints[Random.Range(0, availableWaypoints.Count)];
        occupiedWaypoints[currentTarget] = true;

        agent.SetDestination(currentTarget.position);
    }

    IEnumerator SmoothFaceTarget(Transform target)
    {
        if (target == null) yield break;

        Vector3 lookDir = target.forward;
        lookDir.y = 0;
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

        transform.rotation = targetRot;
    }
}
