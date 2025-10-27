using UnityEngine;

public class CarControl : MonoBehaviour
{
    public float maxMoveSpeed = 10f;       // Max Spd
    public float acceleration = 5f;        // Spd increase rate
    public float deceleration = 4f;        // Spd decrease rate
    public float turnSpeed = 100f;         // turn rate 
    public float turnSmoothness = 5f;      // turn smooth
    public float driftFactor = 0.95f;      // drift

    private float currentSpeed = 0f;       // current speed
    private float currentTurn = 0f;        // current turn
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate; 
        // Use continuous collision detection to reduce jitter on impacts
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    void FixedUpdate()
    {
        // Read inputs
        float verticalInput = Input.GetAxis("Vertical");       // W/S control
        float horizontalInput = Input.GetAxis("Horizontal");   // A/D control

        float dt = Time.fixedDeltaTime;
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        
        // Compute target forward speed (m/s)
        float targetSpeed = verticalInput * maxMoveSpeed;

        // target speed
        if (Mathf.Abs(targetSpeed) > 0.01f)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * dt);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * dt);
        }

        // Smooth turn speed (deg/s), only when moving
        float targetTurnSpeed = (Mathf.Abs(currentSpeed) > 0.1f) ? horizontalInput * turnSpeed : 0f;
        currentTurn = Mathf.Lerp(currentTurn, targetTurnSpeed, dt * turnSmoothness);

        // Desired forward velocity (m/s)
        Vector3 desiredForwardVel = forward * currentSpeed;

        // Keep some of the existing lateral velocity to simulate drift
        Vector3 currentVel = rb.linearVelocity;
        Vector3 lateralVel = Vector3.Project(currentVel, right) * driftFactor;

        // Compose final velocity and apply
        Vector3 finalVel = desiredForwardVel + lateralVel;
        rb.linearVelocity = finalVel;
        
        // Use angular velocity for smooth, physics-friendly rotation (convert deg/s to rad/s)
        rb.angularVelocity = new Vector3(0f, currentTurn * Mathf.Deg2Rad, 0f);
    }
}
