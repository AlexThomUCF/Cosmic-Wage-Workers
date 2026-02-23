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

    [Header("Kiss Indicator")]
    public Image kissIndicator;
    public float indicatorScaleSpeed = 5f; // how fast it grows/shrinks
    public float maxIndicatorScale = 1f;   // fully visible scale
    public float minIndicatorScale = 0f;   // hidden scale

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
        // Check if looking at a kissable customer
        bool canKiss = CanKissCustomer(out Transform customer);

        if (kissIndicator != null)
        {
            // Target scale based on whether we can kiss
            float targetScale = canKiss ? maxIndicatorScale : minIndicatorScale;

            // Smoothly interpolate current scale toward target scale
            float newScale = Mathf.Lerp(kissIndicator.transform.localScale.x, targetScale, Time.unscaledDeltaTime * indicatorScaleSpeed);
            kissIndicator.transform.localScale = new Vector3(newScale, newScale, newScale);
        }

        // Attempt kiss if pressed
        if (controls.Gameplay.Kiss.WasPressedThisFrame() && canKiss)
        {
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