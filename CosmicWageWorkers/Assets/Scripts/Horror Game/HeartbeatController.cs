using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class HeartbeatController : MonoBehaviour
{
    public Transform enemy;          // Assign in the Inspector
    public float maxDistance = 20f;  // Beyond this, no heartbeat
    public float minDistance = 2f;   // Within this, max intensity
    public float maxVolume = 1f;
    public float minVolume = 0f;
    public float minPitch = 0.8f;
    public float maxPitch = 1.5f;

    private AudioSource heartbeat;

    void Start()
    {
        heartbeat = GetComponent<AudioSource>();
        heartbeat.loop = true;
        heartbeat.Play();
    }

    void Update()
    {
        if (enemy == null) return;

        float distance = Vector3.Distance(transform.position, enemy.position);
        float t = Mathf.InverseLerp(maxDistance, minDistance, distance);

        heartbeat.volume = Mathf.Lerp(minVolume, maxVolume, t);
        heartbeat.pitch = Mathf.Lerp(minPitch, maxPitch, t);
    }
}

