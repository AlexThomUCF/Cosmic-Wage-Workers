using UnityEngine;
using System.Collections;

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

    [Header("Boost Settings")]
    public float boostMultiplier = 2f;      // how much faster the car goes when boosted
    public float boostDuration = 3f;        // how long the boost lasts

    private float currentSpeed;
    private float baseMaxSpeed;
    private bool isBoosted = false;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        baseMaxSpeed = maxSpeed; // remember normal top speed
    }

    void FixedUpdate()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxisRaw("Horizontal");

        ApplyHoverForce();
        MoveForward(verticalInput);
        Turn(horizontalInput);
        TiltCar(horizontalInput);
    }

    void ApplyHoverForce()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, hoverHeight * 2f))
        {
            float currentHeight = hit.distance;
            float heightDifference = hoverHeight - currentHeight;

            float force = heightDifference * hoverForce - rb.linearVelocity.y * hoverDamping;
            rb.AddForce(Vector3.up * force);
        }
    }

    void MoveForward(float input)
    {
        float targetSpeed = input * maxSpeed;
        float speedChange = (Mathf.Abs(input) > 0.01f ? acceleration : deceleration) * Time.fixedDeltaTime;
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, speedChange);

        rb.linearVelocity = transform.forward * currentSpeed;
    }

    void Turn(float input)
    {
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
        float targetTilt = -input * tiltAngle;
        Vector3 currentRotation = transform.localEulerAngles;
        float currentZ = currentRotation.z;
        if (currentZ > 180f) currentZ -= 360f;
        float newZ = Mathf.Lerp(currentZ, targetTilt, Time.fixedDeltaTime * tiltSpeed);
        transform.localEulerAngles = new Vector3(currentRotation.x, currentRotation.y, newZ);
    }

    // -------- BOOST SYSTEM --------
    public void ActivateSpeedBoost(float boostMultiplierValue, float duration)
    {
        if (!isBoosted)
            StartCoroutine(SpeedBoostRoutine(boostMultiplierValue, duration));
    }

    private IEnumerator SpeedBoostRoutine(float boostMultiplierValue, float duration)
    {
        isBoosted = true;
        maxSpeed = baseMaxSpeed * boostMultiplierValue;

        yield return new WaitForSeconds(duration);

        maxSpeed = baseMaxSpeed;
        isBoosted = false;
    }
}

