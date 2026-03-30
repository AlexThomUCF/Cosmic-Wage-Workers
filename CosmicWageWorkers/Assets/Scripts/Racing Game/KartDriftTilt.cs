using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class KartDriftTilt : MonoBehaviour
{
    [Header("Kart Visuals")]
    public Transform kartVisualRoot;
    public KartControllerArcade controller;

    [Header("Tilt Settings")]
    public float maxDriftTilt = 15f;
    public float maxSteerTilt = 5f;
    public float tiltSmooth = 5f;

    private float currentTilt = 0f;

    void FixedUpdate()
    {
        if (controller == null) return;

        // Pull inputs from controller
        float steerInput = controller.steerInput;
        bool driftHeld = controller.driftHeld;

        // Determine target tilt
        float targetTilt = 0f;
        if (driftHeld && Mathf.Abs(steerInput) > 0.1f)
            targetTilt = -Mathf.Sign(steerInput) * maxDriftTilt;
        else if (Mathf.Abs(steerInput) > 0.1f)
            targetTilt = -Mathf.Sign(steerInput) * maxSteerTilt;

        currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.fixedDeltaTime * tiltSmooth);

        Vector3 euler = kartVisualRoot.localEulerAngles;
        euler.z = currentTilt;
        kartVisualRoot.localEulerAngles = euler;
    }
}
