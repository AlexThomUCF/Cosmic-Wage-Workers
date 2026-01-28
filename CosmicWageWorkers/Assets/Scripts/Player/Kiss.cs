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

    private Coroutine fadeRoutine;

    private void Awake()
    {
        controls = new PlayerControls();
        audioSource = GetComponent<AudioSource>();
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
            DoKiss();
        }
    }

    private void DoKiss()
    {
        // Play sound
        if (smooch != null)
            audioSource.PlayOneShot(smooch);

        // Play both heart particle systems
        if (pinkHearts != null)
            pinkHearts.Play();

        if (purpleHearts != null)
            purpleHearts.Play();

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