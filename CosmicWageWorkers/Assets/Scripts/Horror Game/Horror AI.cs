using UnityEngine;
using UnityEngine.AI;

public class HorrorAI : MonoBehaviour
{
    NavMeshAgent monsterAgent;
    public Transform playerTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        monsterAgent = GetComponent<NavMeshAgent>();

    }

    // Update is called once per frame
    void Update()
    {
        monsterAgent.SetDestination(playerTransform.position);
    }
}
