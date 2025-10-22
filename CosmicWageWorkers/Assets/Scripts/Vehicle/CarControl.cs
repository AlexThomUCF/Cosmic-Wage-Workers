using UnityEngine;

public class CarControl : MonoBehaviour
{
    public float moveSpeed = 5f;      //AD
    public float turnSpeed = 100f;    //WS
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // WS move
        float move = Input.GetAxis("Vertical") * moveSpeed;

        // AD spin
        float turn = Input.GetAxis("Horizontal") * turnSpeed * Time.fixedDeltaTime;

        // Spin
        rb.MovePosition(rb.position + transform.forward * move * Time.fixedDeltaTime);
        rb.MoveRotation(rb.rotation * Quaternion.Euler(0, turn, 0));
    }
}
