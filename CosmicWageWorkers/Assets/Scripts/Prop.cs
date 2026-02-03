using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Prop : MonoBehaviour
{
    public NavMeshAgent propAgent;
    public Animator animator;
    public GameObject exitArea;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Awake()
    {
        exitArea = GameObject.Find("Exit");
    }
    void Start()
    {
        propAgent = GetComponent<NavMeshAgent>();

        MoveTowardsExit();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void MoveTowardsExit()
    {
        StartCoroutine(StartAnimation());
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 2f, NavMesh.AllAreas))
        {
            propAgent.Warp(hit.position);
            propAgent.SetDestination(exitArea.transform.position);

        }
    }

    IEnumerator StartAnimation()
    {
        animator.SetTrigger("Play");
        yield return new WaitForSeconds(4f);
        propAgent.enabled = true;
    }
}
