using UnityEngine;
using UnityEngine.AI;

public class RunAwayAI : MonoBehaviour
{
    public Transform player;
    public float runDistance = 10f;      // esc distance
    public float repathTime = 0.5f;       // path CD

    private NavMeshAgent agent;
    private float timer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= repathTime)
        {
            timer = 0f;
            RunAway();
        }
    }

    void RunAway()
    {
        Vector3 dir = (transform.position - player.position).normalized;
        Vector3 targetPos = transform.position + dir * runDistance;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPos, out hit, runDistance, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
}
