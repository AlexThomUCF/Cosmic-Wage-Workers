using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private Rigidbody rb;   
    public GameObject checkpoint1;
    public GameObject checkpoint2;
    public GameObject checkpoint3;
    private bool checkpoint1Reached = false;
    private bool checkpoint2Reached = false;
    private bool checkpoint3Reached = false;
    public float fallThreshold;
    public Vector3 startingPosition;
    public Vector3 checkpointPosition1;
    public Vector3 checkpointPosition2;
    public Vector3 checkpointPosition3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    { 
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < fallThreshold && !checkpoint1Reached && !checkpoint2Reached && !checkpoint3Reached)
        {
            transform.position = startingPosition;
            MomentumHalted();

        }

        if (transform.position.y < fallThreshold && checkpoint1Reached && !checkpoint2Reached && !checkpoint3Reached)
        {
            transform.position = checkpointPosition1;
            MomentumHalted();
        }

        if (transform.position.y < fallThreshold && checkpoint1Reached && checkpoint2Reached && !checkpoint3Reached)
        {
            transform.position = checkpointPosition2;
            MomentumHalted();
        }
        if (transform.position.y < fallThreshold && checkpoint1Reached && checkpoint2Reached && checkpoint3Reached)
        {
            transform.position = checkpointPosition3;
            MomentumHalted();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (!checkpoint1Reached && !checkpoint2Reached && !checkpoint3Reached)
            {
                transform.position = startingPosition;
                MomentumHalted();
            }
            else if (checkpoint1Reached && !checkpoint2Reached && !checkpoint3Reached)
            {
                transform.position = checkpointPosition1;
                MomentumHalted();
            }
            else if (checkpoint1Reached && checkpoint2Reached && !checkpoint3Reached)
            {
                transform.position = checkpointPosition2;
                MomentumHalted();
            }
            else if (checkpoint1Reached && checkpoint2Reached && checkpoint3Reached)
            {
                transform.position = checkpointPosition3;
                MomentumHalted();
            }
        }

    }

   private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == checkpoint1)
        {
            checkpoint1Reached = true;
            Debug.Log("Checkpoint 1 reached!");
        }
        else if (collision.gameObject == checkpoint2)
        {
            checkpoint2Reached = true;
            Debug.Log("Checkpoint 2 reached!");
        }
        else if (collision.gameObject == checkpoint3)
        {
            checkpoint3Reached = true;
            Debug.Log("Checkpoint 3 reached!");
        }

    }   

    private void MomentumHalted()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
    }
}
