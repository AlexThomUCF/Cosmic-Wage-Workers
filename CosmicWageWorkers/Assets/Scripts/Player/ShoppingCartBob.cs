using UnityEngine;

public class ShoppingCartBob : MonoBehaviour
{
    [Header("Float Settings")]
    public float floatAmplitude = 0.1f;
    public float floatSpeed = 2f;

    private float startYPosition;
    private float timeOffset;

    void Start()
    {
        startYPosition = transform.position.y;
        timeOffset = Random.Range(0f, 2f * Mathf.PI);
    }

    void Update()
    {
        float newY = startYPosition + Mathf.Sin((Time.time * floatSpeed) + timeOffset) * floatAmplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}