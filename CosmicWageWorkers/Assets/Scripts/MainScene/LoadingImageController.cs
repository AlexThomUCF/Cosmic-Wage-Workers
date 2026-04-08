using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class LoadingImageController : MonoBehaviour
{
    public static LoadingImageController Instance;

    [SerializeField] private Image targetImage;
    [SerializeField] private Canvas targetCanvas;

    private Coroutine fadeCoroutine;

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Start hidden
        HideImageImmediate();
    }

    // Set sprite from NPC
    public void SetSprite(Sprite newSprite)
    {
        if (targetImage == null || newSprite == null) return;
        targetImage.sprite = newSprite;
    }

    // Called when loading starts
    public void ShowImage()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeInImage());
    }

    // Fade in with delay
    private IEnumerator FadeInImage()
    {
        if (targetImage == null) yield break;

        // Ensure canvas is on top
        if (targetCanvas != null)
        {
            targetCanvas.overrideSorting = true;
            targetCanvas.sortingOrder = 100;
        }

        // Start invisible
        Color c = targetImage.color;
        c.a = 0f;
        targetImage.color = c;

        // Delay before showing
        yield return new WaitForSeconds(1.5f);

        float duration = 1f;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;

            // Smooth fade (nicer than linear)
            float t = Mathf.SmoothStep(0f, 1f, timer / duration);

            c.a = t;
            targetImage.color = c;

            yield return null;
        }

        // Ensure fully visible
        c.a = 1f;
        targetImage.color = c;
    }

    // Called when loading ends
    public void HideImage()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        StartCoroutine(FadeOutImage());
    }

    // Optional smooth fade out
    private IEnumerator FadeOutImage()
    {
        if (targetImage == null) yield break;

        float duration = 0.5f;
        float timer = 0f;

        Color c = targetImage.color;
        float startAlpha = c.a;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;

            c.a = Mathf.Lerp(startAlpha, 0f, t);
            targetImage.color = c;

            yield return null;
        }

        c.a = 0f;
        targetImage.color = c;
    }

    // Instant hide (used at startup)
    private void HideImageImmediate()
    {
        if (targetImage == null) return;

        Color c = targetImage.color;
        c.a = 0f;
        targetImage.color = c;
    }
}
