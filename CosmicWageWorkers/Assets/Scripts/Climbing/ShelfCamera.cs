using UnityEngine;

public class ShelfCamera : MonoBehaviour
{
    [Header("Rotation")]
    public float normalTilt = 15f;
    public float lookAheadTilt = 35f;

    [Header("Speed")]
    public float tiltSpeed = 6f;

    [Header("Follow")]
    public Transform leftHand;
    public Transform rightHand;
    public Vector3 cameraOffset = new Vector3(-5f, 0f, 0f); // camera behind the player slightly
    public float followSpeed = 8f;

    private float currentTilt;

    void Start()
    {
        currentTilt = normalTilt;
        SetTilt(currentTilt);
        if (leftHand == null || rightHand == null)
            Debug.LogWarning("Hands not assigned for camera follow!");
    }

    void Update()
    {
        bool lookingAhead = Input.GetKey(KeyCode.Space);
        float targetTilt = lookingAhead ? lookAheadTilt : normalTilt;
        currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.deltaTime * tiltSpeed);
        SetTilt(currentTilt);
    }

    void LateUpdate()
    {
        if (leftHand == null || rightHand == null) return;

        // Compute midpoint between hands
        Vector3 handMid = (leftHand.position + rightHand.position) * 0.5f;

        // Apply offset
        Vector3 targetPos = handMid + cameraOffset;

        // Smooth follow
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * followSpeed);
    }

    void SetTilt(float tilt)
    {
        transform.rotation = Quaternion.Euler(tilt, transform.eulerAngles.y, 0f);
    }

    public bool IsLookingAhead()
    {
        return Input.GetKey(KeyCode.Space);
    }
}