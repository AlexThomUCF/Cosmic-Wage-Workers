using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class KartControllerArcade : MonoBehaviour
{
    [Header("Speed")]
    public float maxSpeed = 25f;
    public float acceleration = 35f;
    public float brake = 45f;
    public float reverseMaxSpeed = 10f;

    [Header("Steering")]
    public float steerStrength = 120f;          // degrees/sec at low speed
    public float steerStrengthAtMax = 70f;      // degrees/sec at max speed
    public float steerResponse = 10f;           // smoothing

    [Header("Grip / Slide")]
    public float forwardGrip = 8f;              // higher = snaps to forward velocity faster
    public float lateralGrip = 6f;              // higher = less side slip
    public float driftLateralGrip = 1.8f;       // lower = more slide while drifting
    public float driftSteerMultiplier = 1.4f;   // stronger steering while drifting
    public float driftExtraYaw = 30f;           // extra rotation feel while drifting

    [Header("Downforce & Stability")]
    public float downforce = 25f;
    public float antiRoll = 2.0f;               // helps stop tipping

    [Header("Boost from Drift")]
    public float driftChargeRate = 1.0f;        // charge/sec
    public float maxDriftCharge = 3.0f;         // cap
    public float boostDuration = 0.8f;
    public float boostAccelMultiplier = 1.6f;
    public float boostMaxSpeedMultiplier = 1.25f;

    [Header("Grounding")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.35f;
    public LayerMask groundMask = ~0;
    public float groundStickForce = 30f;

    private Rigidbody rb;

    public float steerInput;
    private float throttleInput;
    public bool driftHeld;

    private bool isGrounded;
    private float currentYawVel;

    private float driftCharge;
    private float boostTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.centerOfMass = new Vector3(0f, -0.4f, 0f); // helps reduce flipping
    }

    void Update()
    {
        // Old Input system (simple + reliable)
        steerInput = Input.GetAxisRaw("Horizontal");  // A/D
       throttleInput = -Input.GetAxisRaw("Vertical"); // W/S
        driftHeld = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
    }

    void FixedUpdate()
    {
        UpdateGrounded();

        // Apply downforce / ground stick
        if (isGrounded)
        {
            rb.AddForce(-transform.up * downforce, ForceMode.Acceleration);
            rb.AddForce(-transform.up * groundStickForce, ForceMode.Acceleration);
        }

        // Boost timer
        bool boosting = boostTimer > 0f;
        if (boosting) boostTimer -= Time.fixedDeltaTime;

        float speed = rb.linearVelocity.magnitude;
        float speed01 = Mathf.Clamp01(speed / maxSpeed);

        // Steering strength decreases slightly at high speed (more stable)
        float steerDegPerSec = Mathf.Lerp(steerStrength, steerStrengthAtMax, speed01);
        float steerMult = driftHeld ? driftSteerMultiplier : 1f;

        // Extra drift yaw for that “snap” feel
        float yawExtra = driftHeld ? driftExtraYaw * Mathf.Sign(steerInput) : 0f;

        // Smooth steering
        float targetYaw = (steerInput * steerDegPerSec * steerMult) + yawExtra;
        currentYawVel = Mathf.Lerp(currentYawVel, targetYaw, 1f - Mathf.Exp(-steerResponse * Time.fixedDeltaTime));

        // Only steer when moving a bit (or allow small steering at 0)
        float steerFactor = Mathf.Clamp01(speed / 2f);
        transform.Rotate(0f, currentYawVel * steerFactor * Time.fixedDeltaTime, 0f);

        // Throttle / brake forces (in forward direction)
        float accelNow = acceleration;
        float maxSpeedNow = maxSpeed;

        if (boosting)
        {
            accelNow *= boostAccelMultiplier;
            maxSpeedNow *= boostMaxSpeedMultiplier;
        }

        // Forward/back target
        Vector3 forward = transform.forward;

        if (throttleInput > 0.01f)
        {
            // accelerate forward
            rb.AddForce(forward * (throttleInput * accelNow), ForceMode.Acceleration);
        }
        else if (throttleInput < -0.01f)
        {
            // braking / reverse
            if (Vector3.Dot(rb.linearVelocity, forward) > 0.5f)
            {
                rb.AddForce(forward * (throttleInput * brake), ForceMode.Acceleration);
            }
            else
            {
                // reverse cap
                rb.AddForce(forward * (throttleInput * accelNow), ForceMode.Acceleration);
            }
        }

        // Clamp forward speed (separate forward vs reverse)
        ClampSpeed(maxSpeedNow);

        // Grip model: reduce sideways velocity
        ApplyGrip(boosting);

        // Drift charge system
        HandleDriftChargeAndBoost();
        
        // Anti-roll (simple)
        rb.angularVelocity = new Vector3(rb.angularVelocity.x * (1f - antiRoll * Time.fixedDeltaTime),
                                        rb.angularVelocity.y,
                                        rb.angularVelocity.z * (1f - antiRoll * Time.fixedDeltaTime));
    }

    private void UpdateGrounded()
    {
        if (groundCheck == null)
        {
            // fallback: treat as grounded
            isGrounded = true;
            return;
        }

        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask, QueryTriggerInteraction.Ignore);
    }

    private void ClampSpeed(float maxSpeedNow)
    {
        Vector3 v = rb.linearVelocity;
        Vector3 forward = transform.forward;

        float forwardSpeed = Vector3.Dot(v, forward);
        float lateralSpeed = Vector3.Dot(v, transform.right);

        // cap forward
        float cappedForward = Mathf.Clamp(forwardSpeed, -reverseMaxSpeed, maxSpeedNow);

        // rebuild velocity from forward + right components
        rb.linearVelocity = forward * cappedForward + transform.right * lateralSpeed + Vector3.Project(v, Vector3.up);
    }

    private void ApplyGrip(bool boosting)
    {
        Vector3 v = rb.linearVelocity;
        Vector3 upVel = Vector3.Project(v, Vector3.up);
        Vector3 planar = v - upVel;

        Vector3 forward = transform.forward;
        Vector3 right = transform.right;

        float f = Vector3.Dot(planar, forward);
        float r = Vector3.Dot(planar, right);

        // While drifting, allow more lateral slip
        float latGrip = driftHeld ? driftLateralGrip : lateralGrip;

        // Pull sideways velocity toward 0
        r = Mathf.Lerp(r, 0f, 1f - Mathf.Exp(-latGrip * Time.fixedDeltaTime));

        // Pull forward velocity toward aligned forward (reduces weird skids)
        // (keep the magnitude but align direction slightly)
        Vector3 newPlanar = forward * f + right * r;
        rb.linearVelocity = newPlanar + upVel;

        // Extra “forward snap” when not drifting
        if (!driftHeld && !boosting)
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, forward * Vector3.Dot(rb.linearVelocity, forward) + upVel,
                                             1f - Mathf.Exp(-forwardGrip * Time.fixedDeltaTime));
        }
    }

    private void HandleDriftChargeAndBoost()
    {
        // Charge only if grounded + moving a bit + drifting + steering
        float speed = rb.linearVelocity.magnitude;

        if (isGrounded && driftHeld && speed > 5f && Mathf.Abs(steerInput) > 0.15f)
        {
            driftCharge = Mathf.Min(maxDriftCharge, driftCharge + driftChargeRate * Time.fixedDeltaTime);
        }

        // If drift released, spend charge as a boost
        // Detect release with simple state
        // (We’ll track using a local static, but kept simple here)
        // Better: store previous driftHeld in a field
    }

    private bool prevDriftHeld;

    void LateUpdate()
    {
        // Detect drift release here (after Update read)
        if (prevDriftHeld && !driftHeld)
        {
            float t = Mathf.InverseLerp(0.4f, maxDriftCharge, driftCharge);
            if (t > 0f)
            {
                // scale duration a bit by charge
                boostTimer = Mathf.Lerp(0.35f, boostDuration, t);
            }
            driftCharge = 0f;
        }

        prevDriftHeld = driftHeld;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
