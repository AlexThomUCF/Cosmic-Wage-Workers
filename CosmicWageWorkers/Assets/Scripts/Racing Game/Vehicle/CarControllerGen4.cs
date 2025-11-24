using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class VehicleGen4_Arcade : MonoBehaviour
{
    [Header("Wheel Colliders")]
    public WheelCollider frontLeft;
    public WheelCollider frontRight;
    public WheelCollider rearLeft;
    public WheelCollider rearRight;

    [Header("Driving")]
    public float maxMotorTorque = 2000f;      // Stronger accel
    public float maxSteerAngle = 40f;         // Sharper steering
    public float maxSpeedKph = 180f;          // Soft speed cap
    public float yawAssist = 120f;            // Torque to spin car faster
    public float driftFactor = 0.9f;          // How much grip to lose when turning hard

    [Header("Physics")]
    public Vector3 centerOfMassOffset = new Vector3(0f, -0.25f, 0f);

    public Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.centerOfMass += centerOfMassOffset;

        // Wheel setup
        ConfigureSubsteps(frontLeft);
        ConfigureSubsteps(frontRight);
        ConfigureSubsteps(rearLeft);
        ConfigureSubsteps(rearRight);
        SetupFriction();
    }

    void FixedUpdate()
    {
        float v = Input.GetAxis("Vertical");   // W/S
        float hInput = Input.GetAxis("Horizontal"); // A/D

        float speed = rb.linearVelocity.magnitude;

        // Speed-based steering and yaw scaling
        float steerSpeedFactor = Mathf.Clamp01(1f - speed / 50f); // less steering at higher speeds
        float steer = hInput * Mathf.Lerp(maxSteerAngle * 0.6f, maxSteerAngle, steerSpeedFactor);
        frontLeft.steerAngle = steer;
        frontRight.steerAngle = steer;

        // Throttle and soft speed cap
        float motor = v * maxMotorTorque;
        float speedKph = speed * 3.6f;
        float forwardSpeed = Vector3.Dot(rb.linearVelocity, transform.forward);
        if (speedKph > maxSpeedKph && Mathf.Sign(v) == Mathf.Sign(forwardSpeed))
            motor = 0f;

        rearLeft.motorTorque = motor;
        rearRight.motorTorque = motor;

        // 🌀 Scaled Yaw Assist: much lower, fades at higher speed
        float yawScale = Mathf.Lerp(0.2f, 1f, steerSpeedFactor); // low speed = full assist, high = weak
                                                                 // --- Controlled yaw assist (no spin buildup) ---
        float yawVel = rb.angularVelocity.y; // current rotational speed (radians/sec)
        float yawLimit = 1.5f;                // max spin rate before we stop adding torque

        if (Mathf.Abs(yawVel) < yawLimit)
        {
            rb.AddTorque(Vector3.up * hInput * yawAssist * yawScale, ForceMode.Acceleration);
        }
        else
        {
            // Apply gentle counter torque to slow spin if over limit
            rb.AddTorque(Vector3.up * -yawVel * 0.2f, ForceMode.Acceleration);
        }


        // Drift control (rear grip reduction)
        AdjustDrift(Mathf.Abs(hInput));

        SetBrake(0f);
    }


    private void AdjustDrift(float steerInput)
    {
        float rearGrip = Mathf.Lerp(1.0f, driftFactor, steerInput);

        WheelFrictionCurve rearSideFriction = rearLeft.sidewaysFriction;
        rearSideFriction.stiffness = rearGrip;
        rearLeft.sidewaysFriction = rearSideFriction;
        rearRight.sidewaysFriction = rearSideFriction;
    }

    private void SetBrake(float torque)
    {
        frontLeft.brakeTorque = torque;
        frontRight.brakeTorque = torque;
        rearLeft.brakeTorque = torque;
        rearRight.brakeTorque = torque;
    }

    private static void ConfigureSubsteps(WheelCollider wc)
    {
        if (wc == null) return;
        wc.ConfigureVehicleSubsteps(5f, 12, 15);
    }

    private void SetupFriction()
    {
        // Grippy front tires, looser rears for drift feel
        WheelFrictionCurve frontSide = frontLeft.sidewaysFriction;
        frontSide.stiffness = 1.5f;
        frontLeft.sidewaysFriction = frontSide;
        frontRight.sidewaysFriction = frontSide;

        WheelFrictionCurve rearSide = rearLeft.sidewaysFriction;
        rearSide.stiffness = 1.0f;
        rearLeft.sidewaysFriction = rearSide;
        rearRight.sidewaysFriction = rearSide;
    }
}

