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
    }

    void FixedUpdate()
    {
        float verticalInput = Input.GetAxis("Vertical");       // W/S control
        float horizontalInput = Input.GetAxis("Horizontal");   // A/D control

        
        float targetSpeed = verticalInput * maxMoveSpeed;

        // target speed
        if (Mathf.Abs(targetSpeed) > 0.01f)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, deceleration * Time.fixedDeltaTime);
        }

        // smooth turn
        float targetTurn = (Mathf.Abs(currentSpeed) > 0.1f) ? horizontalInput * turnSpeed * Time.fixedDeltaTime : 0f;
        currentTurn = Mathf.Lerp(currentTurn, targetTurn, Time.fixedDeltaTime * turnSmoothness);

        // drift
        Vector3 forwardMovement = transform.forward * currentSpeed * Time.fixedDeltaTime;
        Vector3 sidewaysDrift = transform.right * Vector3.Dot(rb.linearVelocity, transform.right) * driftFactor;
        Vector3 finalVelocity = forwardMovement + sidewaysDrift;

        
        rb.MovePosition(rb.position + finalVelocity);
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0, currentTurn, 0));
    }
}
