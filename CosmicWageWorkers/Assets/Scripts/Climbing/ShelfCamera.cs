using UnityEngine;

public class ShelfCamera : MonoBehaviour
{
    [Header("Rotation")]
    public float normalTilt = 15f;
    public float lookAheadTilt = 35f;

    [Header("Speed")]
    public float tiltSpeed = 6f;

    private float currentTilt;

    void Start()
    {
        currentTilt = normalTilt;
        SetTilt(currentTilt);
    }

    void Update()
    {
        bool lookingAhead = Input.GetKey(KeyCode.Space);

        float targetTilt = lookingAhead ? lookAheadTilt : normalTilt;
        currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.deltaTime * tiltSpeed);

        SetTilt(currentTilt);
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
