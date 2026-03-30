using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PM : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource jumpSound;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 15f;
    [SerializeField] private float acceleration = 25f;
    [SerializeField] private float friction = 20f;
    [SerializeField] private float jumpForce = 8f;
    [SerializeField] private float jumpCooldown = 0.1f;

    [Header("Camera")]
    [SerializeField] private Transform cameraTransform;

    [Header("Gravity")]
    [SerializeField] private float gravityMultiplier = 1.5f;
    [SerializeField] private float groundDrag = 5f;
    [SerializeField] private float groundGracePeriod = 0.1f;

    private Rigidbody rb;
    public Vector2 moveInput;
    private bool isGrounded;
    private float jumpCooldownTimer;
    private float groundedTimer;

    public ParticleSystem fartParticles;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    private void Update()
    {
        jumpCooldownTimer -= Time.deltaTime;
        groundedTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (NPC.isInDialogue)
        {
            return;
        }

        Move();
        ApplyGravity();
        ApplyGroundDrag();
    }

    private void Move()
    {
        // Get camera-relative movement direction
        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0;
        camForward.Normalize();

        Vector3 camRight = cameraTransform.right;
        camRight.y = 0;
        camRight.Normalize();

        Vector3 moveDir = (camForward * moveInput.y + camRight * moveInput.x).normalized;

        // Get current horizontal velocity
        Vector3 currentVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        Vector3 targetVelocity = moveDir * moveSpeed;

        // Apply acceleration/friction for smoother movement
        Vector3 velocityDifference = targetVelocity - currentVelocity;
        float accelerationRate = moveDir.magnitude > 0 ? acceleration : friction;
        Vector3 newVelocity = currentVelocity + velocityDifference * accelerationRate * Time.fixedDeltaTime;

        // Apply new horizontal velocity while preserving vertical
        rb.linearVelocity = new Vector3(newVelocity.x, rb.linearVelocity.y, newVelocity.z);
    }

    private void ApplyGravity()
    {
        // Apply custom gravity for bouncier feel
        rb.linearVelocity += Vector3.down * Physics.gravity.magnitude * gravityMultiplier * Time.fixedDeltaTime;
    }

    private void ApplyGroundDrag()
    {
        // Apply drag when grounded to reduce bouncing on slopes
        bool shouldDrag = isGrounded || groundedTimer > 0f;
        
        if (shouldDrag)
        {
            rb.linearVelocity = new Vector3(
                rb.linearVelocity.x * (1f - groundDrag * Time.fixedDeltaTime),
                rb.linearVelocity.y,
                rb.linearVelocity.z * (1f - groundDrag * Time.fixedDeltaTime)
            );
        }
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed && (isGrounded || groundedTimer > 0f) && jumpCooldownTimer <= 0f)
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            jumpSound.Play();
            jumpCooldownTimer = jumpCooldown;
            groundedTimer = 0f;
        }
    }

    public void SetSpeedMultiplier(float multiplier)
    {
        moveSpeed *= Mathf.Max(0f, multiplier);
    }

    public void ForceAirborne()
    {
        isGrounded = false;
        groundedTimer = 0f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        isGrounded = true;
        groundedTimer = groundGracePeriod;
    }

    private void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boost"))
        {
            fartParticles.Play();
            GravitySFX gravitySFXScript = GameObject.Find("AudioManager").GetComponent<GravitySFX>();
            gravitySFXScript.clipAudioSource.PlayOneShot(gravitySFXScript.fartSFX);
        }
    }
}
