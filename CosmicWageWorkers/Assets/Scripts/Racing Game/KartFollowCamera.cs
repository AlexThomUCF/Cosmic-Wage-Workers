using UnityEngine;

public class KartFollowCamera : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Position")]
    public Vector3 followOffset = new Vector3(0f, 4.5f, -7.5f); 
    public float positionSmooth = 10f;

    [Header("Look")]
    public Vector3 lookOffset = new Vector3(0f, 1.5f, 0f);
    public float rotationSmooth = 12f;

    [Header("Extra Feel")]
    public float speedFovBoost = 0.25f;     
    public float maxFovBoost = 12f;
    public float baseFov = 60f;

    [Header("FOV Particle")]
    public ParticleSystem fovParticle;
    public float fovThreshold = 70f; // FOV value above which particles activate

    private Camera cam;
    private Vector3 velocity;
    private bool particlePlaying = false;

    void Awake()
    {
        cam = GetComponent<Camera>();
        if (cam != null) cam.fieldOfView = baseFov;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // --- Smooth position ---
        Vector3 desiredPos = target.TransformPoint(followOffset);
        transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref velocity, 1f / Mathf.Max(1f, positionSmooth));

        // --- Smooth rotation (look at target) ---
        Vector3 lookPoint = target.position + lookOffset;
        Quaternion desiredRot = Quaternion.LookRotation(lookPoint - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, 1f - Mathf.Exp(-rotationSmooth * Time.deltaTime));

        // --- FOV boost ---
        float speed = 0f;
        var rb = target.GetComponent<Rigidbody>() ?? target.GetComponentInChildren<Rigidbody>();
        if (rb != null) speed = rb.linearVelocity.magnitude;

        float boost = Mathf.Clamp(speed * speedFovBoost, 0f, maxFovBoost);
        float targetFov = baseFov + boost;
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, targetFov, 1f - Mathf.Exp(-8f * Time.deltaTime));

        // --- FOV Particle logic ---
        if (fovParticle != null)
        {
            if (cam.fieldOfView >= fovThreshold && !particlePlaying)
            {
                fovParticle.Play();
                particlePlaying = true;
            }
            else if (cam.fieldOfView < fovThreshold && particlePlaying)
            {
                fovParticle.Stop();
                particlePlaying = false;
            }
        }
    }
}

