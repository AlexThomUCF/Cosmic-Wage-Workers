using UnityEngine;

public class FloatingInSlime : MonoBehaviour
{
    [SerializeField] private float speed = 2.0f;    // How fast the object moves
    [SerializeField] private float height = 0.5f;   // How far up and down it goes
    
    private Vector3 startPosition;

    void Start()
    {
        // Store the original position of the object
        startPosition = transform.localPosition;
    }

    void Update()
    {
        // Calculate the new Y position using a sine wave
        float newY = startPosition.y + Mathf.Sin(Time.time * speed) * height;

        // Apply the new position while keeping X and Z the same
        transform.localPosition = new Vector3(startPosition.x, newY, startPosition.z);
    }
}

