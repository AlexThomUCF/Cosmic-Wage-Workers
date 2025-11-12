using UnityEngine;

public class PlayerRotation : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float rotationSpeed = 10f;

    void Update()
    {
        // Only rotate on Y axis (yaw)
        Vector3 lookDir = cameraTransform.forward;
        lookDir.y = 0f;

        if (lookDir.sqrMagnitude > 0.001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(lookDir);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }
    }
}