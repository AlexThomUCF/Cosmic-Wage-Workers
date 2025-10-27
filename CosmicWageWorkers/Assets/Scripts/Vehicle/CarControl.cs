using UnityEngine;

public class CarControl : MonoBehaviour
{
    public float maxMoveSpeed = 10f;       // Max forward speed (m/s)
    public float acceleration = 5f;        // Acceleration (m/s^2)
    public float deceleration = 4f;        // Deceleration when no input (m/s^2)
    public float turnSpeed = 100f;         // Yaw speed at full steer (deg/s)
    public float minTurnSpeed = 0.1f;      // Minimum forward/back speed required to allow turning (m/s)

    private float currentSpeed;            // Current forward speed (m/s)
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
    }

    void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;

        // Inputs (raw horizontal for instant steering)
        float v = Input.GetAxis("Vertical");
        float h = Input.GetAxisRaw("Horizontal");

        // Forward speed: accelerate towards target when input exists, otherwise decelerate to 0
        float targetSpeed = v * maxMoveSpeed;
        float step = (Mathf.Abs(v) > 0.01f ? acceleration : deceleration) * dt;
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, step);

        // Apply linear velocity (only forward/back)
        rb.linearVelocity = transform.forward * currentSpeed;

        // Only allow turning when moving straight forward/back (realistic)
        bool canTurn = Mathf.Abs(currentSpeed) > minTurnSpeed; // require some forward/back speed
        float yawDegPerSec = canTurn ? (h * turnSpeed) : 0f;
        rb.angularVelocity = Vector3.up * (yawDegPerSec * Mathf.Deg2Rad);
    }
}
