using UnityEngine;
using UnityEngine.AI;

public class HorrorAI : MonoBehaviour
{
    NavMeshAgent monsterAgent;
    public Transform playerTransform;

    private EnemyVision enemyVision;

    private GameObject newObject;
    private int tempMark = 0;
    private bool hasDetected = false;
    //public Transform childTransform;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    //Three states
    //Base roam State
    // Player range state
    // Phase 2 state'

    enum AIState
    {
        Base,
        ZoneState,
        Phase2State
    };

    void Start()
    {
        monsterAgent = GetComponent<NavMeshAgent>();
        enemyVision = GetComponent<EnemyVision>();

    }

    // Update is called once per frame
    void Update()
    {
        if(enemyVision.allTrue)
        {
            monsterAgent.isStopped = false;
            monsterAgent.SetDestination(playerTransform.position);
        }
        else
        {
            monsterAgent.isStopped = true;
        }
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
