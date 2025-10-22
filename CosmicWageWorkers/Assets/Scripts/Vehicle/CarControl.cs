using UnityEngine;

public class CarControl : MonoBehaviour
{
    public float moveSpeed = 5f;      // W/S control forward/backward
    public float turnSpeed = 100f;    // A/D control rotation
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Get input values
        float verticalInput = Input.GetAxis("Vertical");     // W/S
        float horizontalInput = Input.GetAxis("Horizontal"); // A/D

        // lock AD when WS not pressed
        float move = verticalInput * moveSpeed;
        float turn = (verticalInput != 0) ? horizontalInput * turnSpeed * Time.fixedDeltaTime : 0f;

        // Move and turn
        rb.MovePosition(rb.position + transform.forward * move * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0, turn, 0));
    }
}
