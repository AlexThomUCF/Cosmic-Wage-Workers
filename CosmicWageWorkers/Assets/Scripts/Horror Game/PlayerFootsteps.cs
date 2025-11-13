using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayerFootsteps : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerMovement playerMovement; 
    [SerializeField] private Transform groundCheck;

    [Header("Footstep Settings")]
    [SerializeField] private AudioClip[] footstepClips;
    [SerializeField] private float stepInterval = 0.5f;
    [SerializeField] private float moveThreshold = 0.2f;
    [SerializeField] private float groundCheckRadius = 0.25f;
    [SerializeField] private LayerMask groundMask;

    private AudioSource audioSource;
    private float stepTimer = 0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;

        if (playerMovement == null)
            playerMovement = GetComponent<PlayerMovement>();
        if (groundCheck == null && playerMovement != null)
            groundCheck = playerMovement.transform;
    }

    void Update()
    {
        if (playerMovement == null || footstepClips.Length == 0)
            return;

        bool isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundMask);
        Vector2 moveInput = playerMovement.moveInput;
        bool isMoving = moveInput.magnitude > moveThreshold;

        if (isGrounded && isMoving)
        {
            stepTimer -= Time.deltaTime;
            if (stepTimer <= 0f)
            {
                PlayFootstep();
                stepTimer = stepInterval;
            }
        }
        else
        {
            // Reset the timer immediately so footsteps stop right away
            stepTimer = 0f;

            // Stop any looping sound just in case (precaution)
            if (audioSource.isPlaying && !isMoving)
                audioSource.Stop();
        }
    }

    private void PlayFootstep()
    {
        AudioClip clip = footstepClips[Random.Range(0, footstepClips.Length)];
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.PlayOneShot(clip);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
