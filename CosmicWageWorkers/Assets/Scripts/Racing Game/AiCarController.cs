using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(CarControl))]

public class AiCarController : MonoBehaviour
{
    public WaypointContainer waypointContainer;
    public List<Transform> waypoints;
    public int currentWaypoint;
    private CarControl carControl;
    public float waypointRange;
    private float currentAngle;
    private float gasInput;
    public float gasDampen;
    public bool isInsideBraking;
    public float maximumAngle = 45f;
    public float maximumSpeed = 120f;
    public float turningConstant = 0.02f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        carControl = GetComponent<CarControl>();
        waypoints = waypointContainer.waypoints;
        currentWaypoint = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(waypoints[currentWaypoint].position,transform.position) < waypointRange)
        {
            currentWaypoint++;
            if (currentWaypoint == waypoints.Count) currentWaypoint = 0;  
        }
        Vector3 fwd = transform.TransformDirection(Vector3.forward); //Forward vector of ai car
        currentAngle = Vector3.SignedAngle(fwd, waypoints[currentWaypoint].position - transform.position, Vector3.up); //the angle between car and the waypoint
        // carControl.TiltCar(currentAngle);
        gasInput = Mathf.Clamp01(1f - Mathf.Abs(carControl.currentSpeed * turningConstant * currentAngle / (maximumAngle)));
        
        if(isInsideBraking)
        {
            gasInput = -gasInput * ((Mathf.Clamp01((carControl.currentSpeed) / maximumSpeed) * 2 - 1f));
        }
        gasDampen = Mathf.Lerp(gasDampen, gasInput, Time.deltaTime * 3f);
        float turnInput = Mathf.Clamp(currentAngle / 45f, -1f, 1f);
        carControl.MoveForward(gasDampen);
        carControl.Turn(turnInput);

        Debug.DrawRay(transform.position, waypoints[currentWaypoint].position - transform.position, Color.yellow);

    }
}
