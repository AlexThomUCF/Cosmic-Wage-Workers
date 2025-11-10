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

    [Header("Control Mode")]
    public bool usePlayerInput = true; // Toggle for AI or player control

    public float currentSpeed;
    private float currentForwardInput;
    private float currentTurnInput;

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
        if (usePlayerInput)
        {
            currentForwardInput = Input.GetAxis("Vertical");
            currentTurnInput = Input.GetAxisRaw("Horizontal");
        }

        // Handle hovering above ground
        ApplyHoverForce();

        // Handle forward/backward movement
        MoveForward(currentForwardInput);

        // Handle turning
        Turn(currentTurnInput);

        // Tilt the car when turning
        TiltCar(currentTurnInput);
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

    public void MoveForward(float input)
    {
        currentForwardInput = input; // Allow external (AI) control

        // Calculate target speed based on input
        float targetSpeed = input * maxSpeed;

        // Smoothly change current speed
        float speedChange = (Mathf.Abs(input) > 0.01f ? acceleration : deceleration) * Time.fixedDeltaTime;
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, speedChange);

        // ✅ FIX: Preserve existing Y velocity (so gravity & ramps behave correctly)
        rb.linearVelocity = new Vector3(
            transform.forward.x * currentSpeed,
            rb.linearVelocity.y,
            transform.forward.z * currentSpeed
        );
    }

    public void Turn(float input)
    {
        currentTurnInput = input; // Allow external (AI) control

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

    public void TiltCar(float input)
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

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Apply a small repulsion force instead of full physics collision
            Vector3 pushDir = collision.contacts[0].point - transform.position;
            pushDir.y = 0; // only horizontal
            pushDir.Normalize();

            rb.AddForce(-pushDir * 200f); // adjust 200f as needed
        }
    }
}
