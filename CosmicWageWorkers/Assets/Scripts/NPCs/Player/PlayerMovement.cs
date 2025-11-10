using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpForce = 6f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.4f;
    [SerializeField] private LayerMask groundMask;

    private Rigidbody rb;
    private Vector2 moveInput;
    private bool isGrounded;
    private float speedMultiplier = 1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        CheckGrounded();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void CheckGrounded()
    {
        // Check if the sphere overlaps with any collider in the ground layer
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);
    }

    private void Move()
    {
        Vector3 direction = transform.right * moveInput.x + transform.forward * moveInput.y;
        Vector3 targetVelocity = direction * moveSpeed * speedMultiplier;

        // Maintain vertical velocity while applying horizontal movement
        Vector3 velocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);
        rb.linearVelocity = velocity;
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = Mathf.Clamp(multiplier, 0f, 1f);
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = isGrounded ? Color.green : Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}