using UnityEngine;

public class ShelfCamera : MonoBehaviour
{
    [Header("Rotation")]
    public float normalTilt = 15f;
    public float lookAheadTilt = 35f;
    public float tiltSpeed = 6f;

    [Header("Follow")]
    public Transform leftHand;
    public Transform rightHand;
    public Vector3 offset = new Vector3(-5f, 2f, 0f); // behind and slightly above

    public float followSpeed = 8f;

    private float currentTilt;

    void Start()
    {
        currentTilt = normalTilt;
        SetTilt(currentTilt);

        if (leftHand == null || rightHand == null)
            Debug.LogWarning("Hands not assigned!");
    }

    void Update()
    {
        // Tilt logic
        float targetTilt = Input.GetKey(KeyCode.Space) ? lookAheadTilt : normalTilt;
        currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.deltaTime * tiltSpeed);
        SetTilt(currentTilt);
    }

    void LateUpdate()
    {
        if (leftHand == null || rightHand == null) return;

        // Midpoint between hands
        Vector3 midpoint = (leftHand.position + rightHand.position) * 0.5f;

        // Target camera position
        Vector3 targetPos = midpoint + offset;

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