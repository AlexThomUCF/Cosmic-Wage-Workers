using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class KartControllerArcade : MonoBehaviour
{
    [Header("Speed")]
    public float maxSpeed = 25f;
    public float acceleration = 35f;
    public float brake = 45f;
    public float reverseMaxSpeed = 10f;

    [Header("Steering")]
    public float steerStrength = 120f;
    public float steerStrengthAtMax = 70f;
    public float steerResponse = 10f;

    [Header("Grip / Slide")]
    public float forwardGrip = 8f;
    public float lateralGrip = 6f;
    public float driftLateralGrip = 1.8f;
    public float driftSteerMultiplier = 1.4f;
    public float driftExtraYaw = 30f;

    [Header("Downforce & Stability")]
    public float downforce = 25f;
    public float antiRoll = 2.0f;

    [Header("Boost from Drift")]
    public float driftChargeRate = 1.0f;
    public float maxDriftCharge = 3.0f;
    public float boostDuration = 0.8f;
    public float boostAccelMultiplier = 1.6f;
    public float boostMaxSpeedMultiplier = 1.25f;

    [Header("Grounding")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.35f;
    public LayerMask groundMask = ~0;
    public float groundStickForce = 30f;

    [Header("Input Actions")]
    public InputActionReference turnAction;      // Vector2, left/right
    public InputActionReference accelerateAction; // Button
    public InputActionReference reverseAction;    // Button
    public InputActionReference driftAction;      // Button

    [Header("External Effects")]
    public float externalSpeedMultiplier = 1f;
    private float slowTimer = 0f;

    private Rigidbody rb;

    [HideInInspector] public float steerInput;
    [HideInInspector] public float throttleInput;
    [HideInInspector] public bool driftHeld;

    private bool isGrounded;
    private float currentYawVel;
    private float driftCharge;
    private float boostTimer;
    private bool prevDriftHeld;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.centerOfMass = new Vector3(0f, -0.4f, 0f);
    }

    void Update()
    {
        // --- Read Inputs ---
        if (turnAction != null) steerInput = turnAction.action.ReadValue<Vector2>().x;
        if (driftAction != null) driftHeld = driftAction.action.IsPressed();
        if (accelerateAction != null && reverseAction != null)
        {
            throttleInput = (accelerateAction.action.IsPressed() ? 1f : 0f) +
                            (reverseAction.action.IsPressed() ? -1f : 0f);
        }
    }

    void OnEnable()
    {
        turnAction?.action.Enable();
        accelerateAction?.action.Enable();
        reverseAction?.action.Enable();
        driftAction?.action.Enable();
    }

    void OnDisable()
    {
        turnAction?.action.Disable();
        accelerateAction?.action.Disable();
        reverseAction?.action.Disable();
        driftAction?.action.Disable();
    }

    void FixedUpdate()
    {
        // Handle slow timer
        if (slowTimer > 0f)
        {
            slowTimer -= Time.fixedDeltaTime;
            if (slowTimer <= 0f)
            {
                externalSpeedMultiplier = 1f;
            }
        }
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

        // Steering
        float steerDegPerSec = Mathf.Lerp(steerStrength, steerStrengthAtMax, speed01);
        float steerMult = driftHeld ? driftSteerMultiplier : 1f;
        float yawExtra = driftHeld ? driftExtraYaw * Mathf.Sign(steerInput) : 0f;

        float targetYaw = (steerInput * steerDegPerSec * steerMult) + yawExtra;
        currentYawVel = Mathf.Lerp(currentYawVel, targetYaw, 1f - Mathf.Exp(-steerResponse * Time.fixedDeltaTime));

        float steerFactor = Mathf.Clamp01(speed / 2f);
        transform.Rotate(0f, currentYawVel * steerFactor * Time.fixedDeltaTime, 0f);

        // Throttle / brake
        float accelNow = acceleration;
        float maxSpeedNow = maxSpeed * externalSpeedMultiplier;
        if (boosting)
        {
            accelNow *= boostAccelMultiplier;
            maxSpeedNow *= boostMaxSpeedMultiplier;
        }

        Vector3 forward = transform.forward;
        if (throttleInput > 0.01f) rb.AddForce(forward * (throttleInput * accelNow), ForceMode.Acceleration);
        else if (throttleInput < -0.01f)
        {
            if (Vector3.Dot(rb.linearVelocity, forward) > 0.5f)
                rb.AddForce(forward * (throttleInput * brake), ForceMode.Acceleration);
            else
                rb.AddForce(forward * (throttleInput * accelNow), ForceMode.Acceleration);
        }

        ClampSpeed(maxSpeedNow);
        ApplyGrip(boosting);
        HandleDriftChargeAndBoost();

        // Anti-roll
        rb.angularVelocity = new Vector3(rb.angularVelocity.x * (1f - antiRoll * Time.fixedDeltaTime),
                                        rb.angularVelocity.y,
                                        rb.angularVelocity.z * (1f - antiRoll * Time.fixedDeltaTime));
    }

    private void UpdateGrounded()
    {
        if (groundCheck == null) { isGrounded = true; return; }
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask, QueryTriggerInteraction.Ignore);
    }

    private void ClampSpeed(float maxSpeedNow)
    {
        Vector3 v = rb.linearVelocity;
        Vector3 forward = transform.forward;
        float forwardSpeed = Vector3.Dot(v, forward);
        float lateralSpeed = Vector3.Dot(v, transform.right);
        float cappedForward = Mathf.Clamp(forwardSpeed, -reverseMaxSpeed, maxSpeedNow);
        rb.linearVelocity = forward * cappedForward + transform.right * lateralSpeed + Vector3.Project(v, Vector3.up);
    }

    private void ApplyGrip(bool boosting)
    {
        Vector3 v = rb.linearVelocity;
        Vector3 upVel = Vector3.Project(v, Vector3.up);
        Vector3 planar = v - upVel;

        float f = Vector3.Dot(planar, transform.forward);
        float r = Vector3.Dot(planar, transform.right);
        float latGrip = driftHeld ? driftLateralGrip : lateralGrip;
        r = Mathf.Lerp(r, 0f, 1f - Mathf.Exp(-latGrip * Time.fixedDeltaTime));
        Vector3 newPlanar = transform.forward * f + transform.right * r;
        rb.linearVelocity = newPlanar + upVel;

        if (!driftHeld && !boosting)
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, transform.forward * Vector3.Dot(rb.linearVelocity, transform.forward) + upVel,
                                        1f - Mathf.Exp(-forwardGrip * Time.fixedDeltaTime));
        }
    }

    private void HandleDriftChargeAndBoost()
    {
        float speed = rb.linearVelocity.magnitude;
        if (isGrounded && driftHeld && speed > 5f && Mathf.Abs(steerInput) > 0.15f)
            driftCharge = Mathf.Min(maxDriftCharge, driftCharge + driftChargeRate * Time.fixedDeltaTime);
    }

    void LateUpdate()
    {
        if (prevDriftHeld && !driftHeld)
        {
            float t = Mathf.InverseLerp(0.4f, maxDriftCharge, driftCharge);
            if (t > 0f) boostTimer = Mathf.Lerp(0.35f, boostDuration, t);
            driftCharge = 0f;
        }
        prevDriftHeld = driftHeld;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null) Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    public void ApplySlow(float targetSpeed, float duration)
    {
        externalSpeedMultiplier = Mathf.Clamp01(targetSpeed / maxSpeed);
        slowTimer = duration;
        rb.linearVelocity *= 0.05f;
    }
}