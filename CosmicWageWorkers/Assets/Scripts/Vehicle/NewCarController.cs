using UnityEngine;

// Simple car controller using4 WheelColliders
// Controls: W/S (forward/reverse), A/D (steer), Space (brake)
public class NewCarController : MonoBehaviour
{
 [Header("Wheel Colliders")]
 public WheelCollider frontLeft;
 public WheelCollider frontRight;
 public WheelCollider rearLeft;
 public WheelCollider rearRight;

 [Header("Tuning")]
 public float maxMotorTorque =1500f; // Nm applied to driven wheels
 public float maxSteerAngle =30f; // degrees on front wheels
 public float brakeTorque =3000f; // Nm when braking
 public Vector3 centerOfMassOffset = new Vector3(0f, -0.3f,0f);

 private Rigidbody rb;

 void Awake()
 {
 rb = GetComponent<Rigidbody>();
 if (rb != null)
 {
 rb.centerOfMass += centerOfMassOffset;
 rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
 rb.interpolation = RigidbodyInterpolation.Interpolate;
 }

 // Optional: stabilize wheel physics (safe defaults)
 ConfigureSubsteps(frontLeft);
 ConfigureSubsteps(frontRight);
 ConfigureSubsteps(rearLeft);
 ConfigureSubsteps(rearRight);
 }

 void FixedUpdate()
 {
 float v = Input.GetAxis("Vertical"); // W/S
 float h = Input.GetAxis("Horizontal"); // A/D
 bool brake = Input.GetKey(KeyCode.Space);

 // Steering (front wheels)
 float steer = h * maxSteerAngle;
 if (frontLeft != null) frontLeft.steerAngle = steer;
 if (frontRight != null) frontRight.steerAngle = steer;

 // Motor (rear wheels)
 float motor = v * maxMotorTorque;
 if (rearLeft != null) rearLeft.motorTorque = motor;
 if (rearRight != null) rearRight.motorTorque = motor;

 // Brake (all wheels)
 float bt = brake ? brakeTorque :0f;
 if (frontLeft != null) frontLeft.brakeTorque = bt;
 if (frontRight != null) frontRight.brakeTorque = bt;
 if (rearLeft != null) rearLeft.brakeTorque = bt;
 if (rearRight != null) rearRight.brakeTorque = bt;
 }

 private static void ConfigureSubsteps(WheelCollider wc)
 {
 if (wc == null) return;
 wc.ConfigureVehicleSubsteps(5f,12,15);
 }
}
