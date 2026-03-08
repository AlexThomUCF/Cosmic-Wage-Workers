using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class AGPlayerMovement : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource jumpSound;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float jumpForce = 6f;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.4f;
    [SerializeField] private LayerMask groundMask;

    [Header("Camera")]
    [SerializeField] private Transform cameraTransform;

    private Rigidbody rb;
    public Vector2 moveInput;
    private bool isGrounded;
    private float speedMultiplier = 1f;
    private float coyoteTimeCounter;   
    private float coyoteTime = 0.2f; 
    private float jumpBufferCounter;
    private float jumpBufferTime = 0.2f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Keep the player upright
    }

    private void Update()
    {
        CheckGrounded();

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime; // Reset coyote time when grounded
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime; // Decrease coyote time when in the air
            jumpBufferCounter -= Time.deltaTime; // Decrease jump buffer time when in the air
        }
    }

    private void FixedUpdate()
    {
        if(NPC.isInDialogue)
        {
            return; // can't move while talking to npc
        }
        
        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpSound.Play();
            jumpBufferCounter = 0f; // Reset jump buffer after jumping
            coyoteTimeCounter = 0f; // Reset coyote time after jumping
        }
        Move();
    }

    private void CheckGrounded()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);
    }

    private void Move()
    {
        // Project camera forward and right onto XZ plane
        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = cameraTransform.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 moveDir = camForward * moveInput.y + camRight * moveInput.x;
        Vector3 targetVelocity = moveDir * moveSpeed * speedMultiplier;

        // Preserve vertical velocity
        Vector3 velocity = new Vector3(targetVelocity.x, rb.linearVelocity.y, targetVelocity.z);
        rb.linearVelocity = velocity;
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            jumpBufferCounter = jumpBufferTime; // Set jump buffer time when jump is pressed
        }
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        speedMultiplier = Mathf.Max(0f, multiplier); // Only clamp to prevent negative speed
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