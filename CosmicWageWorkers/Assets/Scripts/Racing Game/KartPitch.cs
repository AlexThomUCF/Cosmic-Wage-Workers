using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class KartPitch : MonoBehaviour
{
    public float minPitch = 0.0f;
    public float maxPitch = 2.0f;

    public float minSpeed = 0f;
    public float maxSpeed = 25f;

    public float pitchSmooth = 5f;

    private Rigidbody rb;
    public AudioSource engineAudio;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        

        engineAudio.loop = true;

        if (!engineAudio.isPlaying)
            engineAudio.Play();
    }

    void Update()
    {
        float speed = rb.linearVelocity.magnitude;

        float speed01 = Mathf.InverseLerp(minSpeed, maxSpeed, speed);
        float targetPitch = Mathf.Lerp(minPitch, maxPitch, speed01);

        engineAudio.pitch = Mathf.Lerp(engineAudio.pitch, targetPitch, Time.deltaTime * pitchSmooth);

        engineAudio.volume = Mathf.Lerp(0.3f, 1f, speed01);
    }
}