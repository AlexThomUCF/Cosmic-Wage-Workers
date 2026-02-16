using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Kiss : MonoBehaviour
{
    private PlayerControls controls;

    [Header("Audio")]
    public AudioClip smooch;
    private AudioSource audioSource;

    [Header("Heart Particles")]
    public ParticleSystem pinkHearts;
    public ParticleSystem purpleHearts;

    [Header("Kiss UI")]
    public Image kissImage;

    [Header("Fade Settings")]
    public float kissVisibleTime = 0.3f;
    public float fadeDuration = 1f;

    [Header("Kiss Targeting")]
    public float maxKissDistance = 3f; // max distance to kiss
    public float maxKissAngle = 45f;   // max angle from forward
    public LayerMask customerLayer;    // Layer for customers

    private Coroutine fadeRoutine;
    private Camera playerCam;

    private void Awake()
    {
        controls = new PlayerControls();
        audioSource = GetComponent<AudioSource>();
        playerCam = Camera.main;
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    private void Update()
    {
        if (controls.Gameplay.Kiss.WasPressedThisFrame())
        {
            // Only attempt kiss if facing a customer
            if (CanKissCustomer(out Transform customer))
                DoKiss(customer);
        }
    }

    private bool CanKissCustomer(out Transform customer)
    {
        customer = null;

        // Raycast forward to see if we hit a customer
        Ray ray = new Ray(playerCam.transform.position, playerCam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxKissDistance, customerLayer))
        {
            Vector3 toCustomer = (hit.transform.position - playerCam.transform.position).normalized;
            float angle = Vector3.Angle(playerCam.transform.forward, toCustomer);

            if (angle <= maxKissAngle)
            {
                customer = hit.transform;
                return true;
            }
        }

        return false;
    }

    private void DoKiss(Transform customer)
    {
        // Play sound
        if (smooch != null)
            audioSource.PlayOneShot(smooch);

        // Play both heart particle systems
        if (pinkHearts != null)
            pinkHearts.Play();

        if (purpleHearts != null)
            purpleHearts.Play();

        // Make customer react with a text bubble
        customer.GetComponent<CustomerReaction>()?.ReactToKiss();

        // Restart UI fade if already running
        if (fadeRoutine != null)
            StopCoroutine(fadeRoutine);

        fadeRoutine = StartCoroutine(FadeKiss());
    }

    private IEnumerator FadeKiss()
    {
        SetKissAlpha(1f);
        yield return new WaitForSeconds(kissVisibleTime);

        float t = 0f;
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            SetKissAlpha(Mathf.Lerp(1f, 0f, t / fadeDuration));
            yield return null;
        }

        SetKissAlpha(0f);
    }

    private void SetKissAlpha(float alpha)
    {
        if (kissImage == null) return;

        Color c = kissImage.color;
        c.a = alpha;
        kissImage.color = c;
    }
}