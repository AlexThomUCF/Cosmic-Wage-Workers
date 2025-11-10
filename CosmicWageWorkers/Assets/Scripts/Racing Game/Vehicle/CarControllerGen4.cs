using UnityEngine;

// - W/S: forward/reverse 
// - A/D: steer 

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class VehicleGen4 : MonoBehaviour
{
 [Header("Wheel Colliders")]
 public WheelCollider frontLeft;
 public WheelCollider frontRight;
 public WheelCollider rearLeft;
 public WheelCollider rearRight;

 [Header("Driving")]
 public float maxMotorTorque =1500f; // Nm on rear wheels
 public float maxSteerAngle =60f; // degrees on front wheels
 public float maxSpeedKph =120f; // soft speed cap (km/h)

 [Header("Physics")]
 public Vector3 centerOfMassOffset = new Vector3(0f, -0.3f,0f); // lower for stability

 private Rigidbody rb;

 void Awake()
 {
 rb = GetComponent<Rigidbody>();
 // Basic rigidbody setup
 rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
 rb.interpolation = RigidbodyInterpolation.Interpolate;
 rb.centerOfMass += centerOfMassOffset;

 // Stabilize WheelCollider
 ConfigureSubsteps(frontLeft);
 ConfigureSubsteps(frontRight);
 ConfigureSubsteps(rearLeft);
 ConfigureSubsteps(rearRight);
 }

 void FixedUpdate()
 {
 // Input
 float v = Input.GetAxis("Vertical"); // W/S
 float h = Input.GetAxis("Horizontal"); // A/D

 // Steering (front wheels)
 float steer = h * maxSteerAngle;
 if (frontLeft != null) frontLeft.steerAngle = steer;
 if (frontRight != null) frontRight.steerAngle = steer;

 // Motor (rear wheels)
 float motor = v * maxMotorTorque;

 // Soft speed cap, limit thrust when already at/above cap in same direction
 float speedKph = rb.linearVelocity.magnitude *3.6f;
 float forwardSpeed = Vector3.Dot(rb.linearVelocity, transform.forward);
 if (speedKph > maxSpeedKph && Mathf.Sign(v) == Mathf.Sign(forwardSpeed))
 {
 motor =0f;
 }

 if (rearLeft != null) rearLeft.motorTorque = motor;
 if (rearRight != null) rearRight.motorTorque = motor;

 // Keep brakes off for WASD control
 SetBrake(0f);
 }

 private void SetBrake(float torque)
 {
 if (frontLeft != null) frontLeft.brakeTorque = torque;
 if (frontRight != null) frontRight.brakeTorque = torque;
 if (rearLeft != null) rearLeft.brakeTorque = torque;
 if (rearRight != null) rearRight.brakeTorque = torque;
 }

 private static void ConfigureSubsteps(WheelCollider wc)
 {
 if (wc == null) return;
 wc.ConfigureVehicleSubsteps(5f,12,15);
 }
}
