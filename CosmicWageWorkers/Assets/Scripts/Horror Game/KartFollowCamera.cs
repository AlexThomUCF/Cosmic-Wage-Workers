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

    [Header("FOV Particles")]
    public ParticleSystem fovParticles;
    public float fovParticleTrigger = 79f;

    private Camera cam;
    private Vector3 velocity;
    private bool particlesPlaying;

    void Awake()
    {
        cam = GetComponent<Camera>();
        if (cam != null)
            cam.fieldOfView = baseFov;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Smooth position
        Vector3 desiredPos = target.TransformPoint(followOffset);
        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPos,
            ref velocity,
            1f / Mathf.Max(1f, positionSmooth)
        );

        // Smooth rotation
        Vector3 lookPoint = target.position + lookOffset;
        Quaternion desiredRot = Quaternion.LookRotation(lookPoint - transform.position, Vector3.up);
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            desiredRot,
            1f - Mathf.Exp(-rotationSmooth * Time.deltaTime)
        );

        // FOV boost with speed
        if (cam != null && speedFovBoost > 0f)
        {
            float speed = 0f;
            Rigidbody rb = target.GetComponent<Rigidbody>();
            if (rb == null)
                rb = target.GetComponentInChildren<Rigidbody>();

            if (rb != null)
                speed = rb.linearVelocity.magnitude;

            float boost = Mathf.Clamp(speed * speedFovBoost, 0f, maxFovBoost);
            cam.fieldOfView = Mathf.Lerp(
                cam.fieldOfView,
                baseFov + boost,
                1f - Mathf.Exp(-8f * Time.deltaTime)
            );
        }

        // FOV particle trigger
        if (fovParticles != null && cam != null)
        {
            if (cam.fieldOfView > fovParticleTrigger)
            {
                if (!particlesPlaying)
                {
                    fovParticles.Play();
                    particlesPlaying = true;
                }
            }
            else
            {
                if (particlesPlaying)
                {
                    fovParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
                    particlesPlaying = false;
                }
            }
        }
    }
}

