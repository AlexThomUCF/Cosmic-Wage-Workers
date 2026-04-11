using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] public float moveSpeed = 10f;
    [SerializeField] private float jumpForce = 6f;

    [Header("Dash Settings")]
    private bool isDashing;
    private Vector3 dashTargetVelocity;

    [SerializeField] public float dashSpeed = 12f;
    [SerializeField] public float dashSmoothness = 10f;
    [SerializeField] public float dashDuration = 0.2f;

    private float dashTimer;



    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckRadius = 0.4f;
    [SerializeField] private LayerMask groundMask;

    [Header("Camera")]
    [SerializeField] private Transform cameraTransform;

    public Transform cameraRoot;

    public float maxRollAngle = 10f;
    public float rollSmoothness = 8f;

    private float targetRoll;



    private Rigidbody rb;
    public Vector2 lookInput;
    public Vector2 moveInput;
    private bool isGrounded;
    [SerializeField] private bool isRole = false;
    private float speedMultiplier = 1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Keep the player upright
    }

    private void Update()
    {
        CheckGrounded();

        if (cameraRoot == null) return;

        // Get current Z rotation
        float currentZ = cameraRoot.localEulerAngles.z;

        // Convert from 0–360 to -180–180
        if (currentZ > 180f) currentZ -= 360f;

        // Smoothly interpolate to target roll
        float smoothedRoll = Mathf.Lerp(
            currentZ,
            isDashing ? targetRoll : 0f,
            rollSmoothness * Time.deltaTime
        );

        // Keep existing X/Y rotations (Cinemachine handles these)
        Vector3 euler = cameraRoot.localEulerAngles;
        cameraRoot.localRotation = Quaternion.Euler(euler.x, euler.y, smoothedRoll);
    }

    private void FixedUpdate()
    {
        if(NPC.isInDialogue)
        {
            return; // can't move while talking to npc
        }
        Move();

        if (isDashing)
        {
            Vector3 currentVelocity = rb.linearVelocity;

            // Preserve vertical movement (gravity)
            Vector3 horizontal = Vector3.Lerp(
                new Vector3(currentVelocity.x, 0, currentVelocity.z),
                dashTargetVelocity,
                dashSmoothness * Time.fixedDeltaTime
            );

            rb.linearVelocity = new Vector3(
                horizontal.x,
                currentVelocity.y,
                horizontal.z
            );

            dashTimer -= Time.fixedDeltaTime;

            if (dashTimer <= 0f)
            {
                isDashing = false;
            }
        }

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

    public void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    public void OnRole(InputValue value)
    {
        if (value.isPressed && isGrounded && isRole)
        {
            // Check if player is actually moving
            Vector3 horizontalVelocity = new Vector3(
                rb.linearVelocity.x,
                0,
                rb.linearVelocity.z
            );

            if (horizontalVelocity.sqrMagnitude < 0.1f)
                return; // Don't dash if not moving

            Vector3 dashDir = horizontalVelocity.normalized;

            // Determine roll direction using dot product
            float side = Vector3.Dot(transform.right, dashDir);

            // Tilt opposite direction for natural feel
            targetRoll = -side * maxRollAngle;

            dashTargetVelocity = dashDir * dashSpeed;

            isDashing = true;
            dashTimer = dashDuration;
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