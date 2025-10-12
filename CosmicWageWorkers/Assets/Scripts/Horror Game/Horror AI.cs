using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class HorrorAI : MonoBehaviour
{
    public NavMeshAgent monsterAgent;
    public Transform playerTransform;

    public Transform centrePoint; // centre of the area the agent wants to move around in
    public float range; //Radius of spehere around agent. 


    private EnemyVision enemyVision;
    private FlashLight flashLight;

    public enum AIState
    {
        NormalState,EnragedState
    };

    void Start()
    {
        monsterAgent = GetComponent<NavMeshAgent>();
        enemyVision = GetComponent<EnemyVision>();
        flashLight = FindAnyObjectByType<FlashLight>();

        currentState = AIState.NormalState;



    }

    public AIState currentState;


    // Update is called once per frame
    void Update()
    {
       switch(currentState)
        {
            case AIState.NormalState:
                if (enemyVision.allTrue)
                {
                    monsterAgent.isStopped = false;
                    monsterAgent.SetDestination(playerTransform.position);
                }


                // StartCoroutine(ChooseAction());
                if (monsterAgent.remainingDistance <= monsterAgent.stoppingDistance) // done with path
                {
                    // ChooseAction(); // when path is done call Choose action command
                    Vector3 point;

                    if (RandomPoint(centrePoint.position, range, out point) && (!enemyVision.allTrue))
                    {
                        Debug.DrawRay(point, Vector3.up, Color.blue, 1.0f);
                        monsterAgent.SetDestination(point);
                        StartCoroutine(ResetAfterMovement());
                    }
                }

                    break;
            case AIState.EnragedState:

                StartCoroutine(WaitForSwap());
                Debug.Log("IN MAD STATE");
                monsterAgent.speed = 20f;
                monsterAgent.SetDestination(playerTransform.position);
                Destroy(flashLight.fLight);
                //Destroy flashlight
                        //Enraged script
                        break;

        }


  
    }
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {

        Vector3 randomPoint = center + Random.insideUnitSphere * range; //random point in sphere
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }

    IEnumerator ResetAfterMovement()
    {
        while (monsterAgent.pathPending || monsterAgent.remainingDistance > monsterAgent.stoppingDistance)
        {
            yield return null; // Wait until the AI reaches its destination
        }
    }

    public void SetState(AIState newState)
    {
        if (currentState == newState) return;

        currentState = newState;

        // Any extra logic when switching states
        Debug.Log("State changed to: " + newState);
    }

    IEnumerator WaitForSwap()
    {
        Debug.Log("Waiting for swap");
        yield return new WaitForSeconds(4f);
    }

    


    /*IEnumerator DetectedAttack()// used just for exclamation mark spawn
    {
        monsterAgent.isStopped = true;

        if (tempMark == 0)
        {

            newObject = Instantiate(noticeMark, childTransform.position, Quaternion.identity);
            //guardAudio.Play();
            newObject.transform.SetParent(childTransform);
            tempMark++;
            Destroy(newObject, 1.8f);
        }

        yield return new WaitForSeconds(1.8f);

        monsterAgent.isStopped = false;
        tempMark = 0;
    }*/
}
