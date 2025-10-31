using UnityEngine;

public class CarControl : MonoBehaviour
{
    [Header("Movement Settings")]
    public float maxSpeed = 10f;
    public float acceleration = 5f;
    public float deceleration = 4f;
    
    [Header("Turning Settings")]
    public float turnSpeed = 100f;
    public float minSpeedToTurn = 0.1f;
    public float tiltAngle = 5f;
    public float tiltSpeed = 5f;
    
    [Header("Hover Settings")]
    public float hoverHeight = 0.5f;
    public float hoverForce = 300f;
    public float hoverDamping = 10f;

    private float currentSpeed;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    void FixedUpdate()
    {
        // Get player input
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        // Handle hovering above ground
        ApplyHoverForce();

        // Handle forward/backward movement
        MoveForward(verticalInput);

        // Handle turning
        Turn(horizontalInput);

        // Tilt the car when turning
        TiltCar(horizontalInput);
    }

    void ApplyHoverForce()
    {
        RaycastHit hit;
        // Cast a ray downward from the car
        if (Physics.Raycast(transform.position, -transform.up, out hit, hoverHeight * 2f))
        {
            // Calculate how far we are from desired hover height
            float currentHeight = hit.distance;
            float heightDifference = hoverHeight - currentHeight;
            
            // Apply upward force to maintain hover height
            float force = heightDifference * hoverForce - rb.linearVelocity.y * hoverDamping;
            rb.AddForce(Vector3.up * force);
        }
    }

    void MoveForward(float input)
    {
        // Calculate target speed based on input
        float targetSpeed = input * maxSpeed;
        
        // Smoothly change current speed
        float speedChange = (Mathf.Abs(input) > 0.01f ? acceleration : deceleration) * Time.fixedDeltaTime;
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, speedChange);

        // Move the car
        rb.linearVelocity = transform.forward * currentSpeed;
    }

    void Turn(float input)
    {
        // Only turn if the car is moving
        if (Mathf.Abs(currentSpeed) > minSpeedToTurn)
        {
            float turnAmount = input * turnSpeed * Mathf.Deg2Rad;
            rb.angularVelocity = Vector3.up * turnAmount;
        }
        else
        {
            rb.angularVelocity = Vector3.zero;
        }
    }

    void TiltCar(float input)
    {
        // Calculate target tilt (roll) based on turning direction
        float targetTilt = -input * tiltAngle;
        
        // Get current rotation
        Vector3 currentRotation = transform.localEulerAngles;
        
        // Normalize the current Z rotation to -180 to 180 range
        float currentZ = currentRotation.z;
        if (currentZ > 180f) currentZ -= 360f;
        
        // Smoothly interpolate to target tilt
        float newZ = Mathf.Lerp(currentZ, targetTilt, Time.fixedDeltaTime * tiltSpeed);
        
        // Apply the new rotation
        transform.localEulerAngles = new Vector3(currentRotation.x, currentRotation.y, newZ);
    }
}
