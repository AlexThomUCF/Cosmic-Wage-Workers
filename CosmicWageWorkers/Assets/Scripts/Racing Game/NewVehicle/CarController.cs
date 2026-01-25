using UnityEngine;
using System;
using System.Collections.Generic;

public class CarController : MonoBehaviour
{
    public enum ControlMode
    {
        Keyboard,
        Buttons
    };

    public enum Axel
    {
        Front,
        Rear
    }

    [Serializable]
    public struct Wheel
    {
        public WheelCollider wheelCollider;
        public Axel axel;
    }

    public ControlMode control;

    // Tune these in Inspector based on your car mass/gearing
    public float maxAcceleration = 30.0f; // Motor torque multiplier (Nm)
    public float brakeAcceleration = 50.0f; // Brake torque multiplier (Nm)

    public float turnSensitivity = 1.0f;
    public float maxSteerAngle = 30.0f; // Degrees

    public Vector3 _centerOfMass; // Set slightly below the chassis center (e.g., y = -0.3)

    public List<Wheel> wheels;

    float moveInput;
    float steerInput;

    private Rigidbody carRb;

    void Start()
    {
        carRb = GetComponent<Rigidbody>();
        carRb.centerOfMass = _centerOfMass;
        carRb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        carRb.interpolation = RigidbodyInterpolation.Interpolate;
    }

    void Update()
    {
        GetInputs();
    }

    void FixedUpdate()
    {
        Move();
        Steer();
        Brake();
    }

    public void MoveInput(float input)
    {
        moveInput = Mathf.Clamp(input, -1f, 1f);
    }

    public void SteerInput(float input)
    {
        steerInput = Mathf.Clamp(input, -1f, 1f);
    }

    void GetInputs()
    {
        if (control == ControlMode.Keyboard)
        {
            moveInput = Input.GetAxis("Vertical");
            steerInput = Input.GetAxis("Horizontal");
        }
    }

    void Move()
    {
        // Apply motor torque on all driven wheels (current setup applies to all wheels)
        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.motorTorque = moveInput * maxAcceleration * 600f; // no deltaTime here
        }
    }

    void Steer()
    {
        foreach (var wheel in wheels)
        {
            if (wheel.axel == Axel.Front)
            {
                var targetAngle = steerInput * turnSensitivity * maxSteerAngle;
                wheel.wheelCollider.steerAngle = Mathf.Lerp(wheel.wheelCollider.steerAngle, targetAngle, 0.6f);
            }
        }
    }

    void Brake()
    {
        bool braking = Input.GetKey(KeyCode.Space) || Mathf.Approximately(moveInput, 0f);
        float torque = braking ? (300f * brakeAcceleration) : 0f; // no deltaTime here

        foreach (var wheel in wheels)
        {
            wheel.wheelCollider.brakeTorque = torque;
        }
    }
}