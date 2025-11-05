using UnityEngine;

// Drive a car that uses4 SphereColliders as wheels (no WheelColliders).
// Applies forces at each wheel contact point based on WASD input.
// Attach to the car body that has a Rigidbody. Assign the4 wheel transforms (each with a SphereCollider).
public class CarControllerGen3 : MonoBehaviour
{
 [Header("Wheels (SphereColliders on these transforms)")]
 public Transform frontLeft;
 public Transform frontRight;
 public Transform rearLeft;
 public Transform rearRight;

 [Header("Drive/Steer Config")]
 public bool frontWheelDrive = true;
 public bool rearWheelDrive = true;
 public bool frontWheelSteer = true;
 public bool rearWheelSteer = false; // usually false

 [Header("Input/Tuning")]
 public float motorForce =3000f; // N total, distributed to drive wheels
 public float maxSteerAngle =30f; // degrees
 public float brakeForce =5000f; // N opposing longitudinal velocity
 public float corneringStiffness =4000f; // N opposing lateral velocity
 public float maxSpeed =30f; // m/s clamp

 [Header("Grounding")]
 public LayerMask groundMask = ~0; // which layers count as ground
 public float contactOffset =0.05f; // extra ray distance beyond sphere radius

 private Rigidbody rb;

 private struct Wheel
 {
 public Transform t;
 public SphereCollider col;
 public bool drive;
 public bool steer;
 }

 private Wheel[] wheels;

 void Awake()
 {
 rb = GetComponent<Rigidbody>();
 if (rb != null)
 {
 rb.interpolation = RigidbodyInterpolation.Interpolate;
 rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
 }

 wheels = new Wheel[4];
 SetupWheel(0, frontLeft, frontWheelDrive, frontWheelSteer);
 SetupWheel(1, frontRight, frontWheelDrive, frontWheelSteer);
 SetupWheel(2, rearLeft, rearWheelDrive, rearWheelSteer);
 SetupWheel(3, rearRight, rearWheelDrive, rearWheelSteer);
 }

 private void SetupWheel(int index, Transform t, bool drive, bool steer)
 {
 if (t == null)
 {
 wheels[index] = new Wheel { t = null, col = null, drive = false, steer = false };
 return;
 }
 var col = t.GetComponent<SphereCollider>();
 if (col == null)
 {
 Debug.LogWarning($"Wheel '{t.name}' is missing SphereCollider.", t);
 }
 wheels[index] = new Wheel { t = t, col = col, drive = drive, steer = steer };
 }

 void FixedUpdate()
 {
 float v = Input.GetAxis("Vertical"); // W/S
 float h = Input.GetAxis("Horizontal"); // A/D
 bool braking = Input.GetKey(KeyCode.Space) || Mathf.Approximately(v,0f);

 // Clamp body speed to avoid runaway
 if (rb.linearVelocity.magnitude > maxSpeed)
 {
 rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
 }

 // Determine how many wheels are driving to split force
 int driveCount =0;
 for (int i =0; i < wheels.Length; i++) if (wheels[i].drive && wheels[i].t != null) driveCount++;
 driveCount = Mathf.Max(1, driveCount);

 for (int i =0; i < wheels.Length; i++)
 {
 var w = wheels[i];
 if (w.t == null || w.col == null) continue;

 float radius = w.col.radius * Mathf.Max(w.t.lossyScale.x, Mathf.Max(w.t.lossyScale.y, w.t.lossyScale.z));

 // Raycast down from wheel center to find ground contact
 Vector3 origin = w.t.position;
 float rayLength = radius + contactOffset +0.5f; // small extra to be resilient
 if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, rayLength, groundMask, QueryTriggerInteraction.Ignore))
 {
 Vector3 contactPoint = hit.point;
 Vector3 groundNormal = hit.normal;

 // Steering direction for this wheel
 float steerDeg = (w.steer ? (h * maxSteerAngle) :0f);
 Quaternion steerRot = Quaternion.AngleAxis(steerDeg, Vector3.up);
 Vector3 wheelForward = steerRot * transform.forward; // use body forward as base
 Vector3 wheelRight = Vector3.Cross(groundNormal, wheelForward).normalized;
 wheelForward = Vector3.Cross(wheelRight, groundNormal).normalized; // re-orthogonalize on ground plane

 // Force at wheel from throttle (distributed across drive wheels)
 if (w.drive)
 {
 Vector3 driveForce = wheelForward * (v * motorForce / driveCount);
 rb.AddForceAtPosition(driveForce, contactPoint, ForceMode.Force);
 }

 // Get velocity of the contact point
 Vector3 pointVelocity = rb.GetPointVelocity(contactPoint);
 float vLong = Vector3.Dot(pointVelocity, wheelForward); // along rolling direction
 float vLat = Vector3.Dot(pointVelocity, wheelRight); // sideways slip

 // Braking (oppose longitudinal motion)
 if (braking)
 {
 Vector3 brake = -vLong * wheelForward * Mathf.Min(Mathf.Abs(vLong),1f) * brakeForce;
 rb.AddForceAtPosition(brake, contactPoint, ForceMode.Force);
 }

 // Lateral friction (oppose side slip)
 Vector3 lateral = -vLat * wheelRight * corneringStiffness;
 rb.AddForceAtPosition(lateral, contactPoint, ForceMode.Force);

 // Optional: small normal force to keep contact stable (acts like simple suspension helper)
 float keepDown =1000f; // tune if needed
 rb.AddForceAtPosition(groundNormal * keepDown, contactPoint, ForceMode.Force);
 }
 }
 }
}
