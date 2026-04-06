using UnityEngine;

public class FartBoost : MonoBehaviour
{
    [Header("Boost Settings")]
    [SerializeField] private Vector3 boostDirection = Vector3.up;
    [SerializeField] private float boostForce = 15f;
    [SerializeField] private float boostDuration = 0.1f;

    [Header("Respawn")]
    [SerializeField] private float respawnTime = 5f;

    [Header("Bob & Rotate")]
    [SerializeField] private float bobHeight = 0.5f;
    [SerializeField] private float bobSpeed = 2f;
    [SerializeField] private float rotationSpeed = 100f;

    [Header("Audio & Effects")]
    [SerializeField] private ParticleSystem boostParticles;
    [SerializeField] private AudioClip boostSound;
    [SerializeField] private AudioSource sfxAudioSource;

    private Collider boostCollider;
    private Renderer boostRenderer;
    private GameObject outlineChild;
    private bool isAvailable = true;
    private float respawnTimer;
    private Vector3 startPosition;

    private void Start()
    {
        boostCollider = GetComponent<Collider>();
        boostRenderer = GetComponent<Renderer>();
        startPosition = transform.position;

        // Find the outline child
        outlineChild = transform.Find("Outline")?.gameObject;

        // If no SFX audio source assigned, create one
        if (sfxAudioSource == null)
        {
            sfxAudioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    private void Update()
    {
        if (isAvailable)
        {
            BobAndRotate();
        }
        else
        {
            respawnTimer -= Time.deltaTime;

            if (respawnTimer <= 0f)
            {
                Respawn();
            }
        }
    }

    private void BobAndRotate()
    {
        // Bob up and down
        float bobOffset = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
        transform.position = startPosition + Vector3.up * bobOffset;

        // Rotate continuously
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f, Space.Self);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isAvailable)
        {
            ActivateBoost(other.GetComponent<Rigidbody>());
            Deactivate();
        }
    }

    private void ActivateBoost(Rigidbody playerRb)
    {
        // Apply directional boost force
        Vector3 normalizedDirection = boostDirection.normalized;
        playerRb.linearVelocity = normalizedDirection * boostForce;

        // Play effects
        if (boostParticles != null)
        {
            boostParticles.Play();
        }

        if (boostSound != null && sfxAudioSource != null)
        {
            sfxAudioSource.PlayOneShot(boostSound);
        }
    }

    private void Deactivate()
    {
        isAvailable = false;
        boostCollider.enabled = false;
        boostRenderer.enabled = false;

        if (outlineChild != null)
        {
            outlineChild.SetActive(false);
        }

        respawnTimer = respawnTime;
    }

    private void Respawn()
    {
        boostCollider.enabled = true;
        boostRenderer.enabled = true;

        if (outlineChild != null)
        {
            outlineChild.SetActive(true);
        }

        startPosition = transform.position;
        isAvailable = true;
    }
}