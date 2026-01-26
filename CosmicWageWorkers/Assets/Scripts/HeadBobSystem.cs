using UnityEngine;
using Unity.Cinemachine;

public class HeadBobSystem : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cinemachineCamera;

    [SerializeField] private NoiseSettings walkProfile;

    [Header("Tuning")]
    [SerializeField] private float walkAmplitude = 1f;
    [SerializeField] private float idleAmplitude = 0f;
    [SerializeField] private float fadeSpeed = 8f;

    private CinemachineBasicMultiChannelPerlin noise;
    private float currentAmplitude;

    void Awake()
    {
        noise = cinemachineCamera.GetComponentInChildren<CinemachineBasicMultiChannelPerlin>();
        noise.NoiseProfile = walkProfile; // set once
    }

    void Update()
    {
        float inputMagnitude = new Vector3(
            Input.GetAxis("Horizontal"),
            0,
            Input.GetAxis("Vertical")
        ).magnitude;

        float targetAmplitude = inputMagnitude > 0.01f
            ? walkAmplitude
            : idleAmplitude;

        currentAmplitude = Mathf.Lerp(
            currentAmplitude,
            targetAmplitude,
            Time.deltaTime * fadeSpeed
        );

        noise.AmplitudeGain = currentAmplitude;
    }
}
