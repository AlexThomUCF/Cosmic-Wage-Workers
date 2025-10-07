using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    public float detectRange = 10;
    public float detectAngle = 45f;
    public GameObject Player;


    [Header("Bools")]
    public bool isInAngle, isInRange, isNotHidden;
    public bool allTrue;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        isInAngle = false;
        isInRange = false;
        isNotHidden = false;

        if (Vector3.Distance(transform.position, Player.transform.position) < detectRange)
        {
            isInRange = true;
            Debug.Log("Player in range");

        }
        else
        {
            Debug.Log("Player out of range");
        }
        BehindObject();
        InAngle();

        //If all true, Bool is true,
        if (isInAngle && isInRange && isNotHidden)
        {
             Debug.Log("ALL TRUE");
            allTrue = true;
        }
        else
        {
            allTrue = false;
        }
        //OnDrawGizmos();
    }

    void BehindObject()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, (Player.transform.position - transform.position), out hit, Mathf.Infinity))//if raycast from enemy to player 
        {
            if (hit.transform == Player.transform) //if hit true
            {
                isNotHidden = true;
                //Debug.Log("Player not hidden");
            }
            else
            {
                // Debug.Log("Player is Hidden");
            }
        }
        else
        {
            // Debug.Log("Hit nothing, can't find player");
        }
    }

    void InAngle()
    {
        Vector3 side1 = Player.transform.position - transform.position;
        Vector3 side2 = transform.forward;
        float angle = Vector3.SignedAngle(side1, side2, Vector3.up);
        if (angle < detectAngle && angle > -1 * detectAngle)
        {
            isInAngle = true;
            //  Debug.Log("In in angle");
        }
        else
        {
            //  Debug.Log("Not in Angle");
        }

    }

    void OnDrawGizmos()
    {
        // Set Gizmo color
        Gizmos.color = Color.red;

        // Draw detection range sphere
        Gizmos.DrawWireSphere(transform.position, detectRange);

        // Get forward direction of enemy
        Vector3 forward = transform.forward * detectRange;

        // Calculate the left and right boundaries of the vision cone
        Quaternion leftRotation = Quaternion.Euler(0, -detectAngle, 0);
        Quaternion rightRotation = Quaternion.Euler(0, detectAngle, 0);

        Vector3 leftDirection = leftRotation * forward;
        Vector3 rightDirection = rightRotation * forward;

        // Draw vision cone lines
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + leftDirection);
        Gizmos.DrawLine(transform.position, transform.position + rightDirection);

        // Draw a line toward the player (for debugging)
        if (Player != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, Player.transform.position);
        }
    }

}