using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingImageController : MonoBehaviour
{
    public static LoadingImageController Instance;

    [SerializeField] private Image targetImage;
    [SerializeField] private Canvas targetCanvas;
    [SerializeField] private TextMeshProUGUI tipText;

    [Header("Fade Settings")]
    [SerializeField] private float showDelay = 1.5f;
    [SerializeField] private float imageFadeInDuration = 1.5f;
    [SerializeField] private float textDelayAfterImageStarts = 0.75f;
    [SerializeField] private float textFadeInDuration = 0.5f;
    [SerializeField] private float fadeOutDuration = 0.5f;

    [Header("Tip Settings")]
    [SerializeField] private float tipChangeInterval = 3f;

    [Header("Final FPS")]
    [SerializeField] public Sprite finalImage;
    [SerializeField] public string[] finalTips;
    


    private Coroutine fadeCoroutine;
    private Coroutine tipCoroutine;

    private string[] currentTips;
    private int currentTipIndex;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Destroy(gameObject);
            return;
        }

        HideImageImmediate();
    }

    public void SetSprite(Sprite newSprite)
    {
        if (targetImage == null || newSprite == null) return;
        targetImage.sprite = newSprite;
    }

    public void SetText(string text)
    {
        if (tipText == null || string.IsNullOrEmpty(text)) return;
        tipText.text = text;
    }

    public void SetTips(string[] tips)
    {
        if (tips == null || tips.Length == 0) return;

        currentTips = tips;
        currentTipIndex = 0;

        SetText(currentTips[currentTipIndex]);

        if (tipCoroutine != null)
            StopCoroutine(tipCoroutine);

        tipCoroutine = StartCoroutine(CycleTips());
    }

    private IEnumerator CycleTips()
    {
        while (currentTips != null && currentTips.Length > 0)
        {
            yield return new WaitForSeconds(tipChangeInterval);

            currentTipIndex++;

            if (currentTipIndex >= currentTips.Length)
                currentTipIndex = 0;

            SetText(currentTips[currentTipIndex]);
        }
    }

    public void ShowImage()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeInImageAndText());
    }

    private IEnumerator FadeInImageAndText()
    {
        if (targetImage == null) yield break;

        if (targetCanvas != null)
        {
            targetCanvas.overrideSorting = true;
            targetCanvas.sortingOrder = 100;
        }

        SetAlpha(targetImage, 0f);
        SetAlpha(tipText, 0f);

        yield return new WaitForSeconds(showDelay);

        float timer = 0f;
        float totalDuration = Mathf.Max(
            imageFadeInDuration,
            textDelayAfterImageStarts + textFadeInDuration
        );

        while (timer < totalDuration)
        {
            timer += Time.deltaTime;

            float imageT = Mathf.Clamp01(timer / imageFadeInDuration);
            float imageAlpha = Mathf.SmoothStep(0f, 1f, imageT);
            SetAlpha(targetImage, imageAlpha);

            if (timer >= textDelayAfterImageStarts)
            {
                float textTimer = timer - textDelayAfterImageStarts;
                float textT = Mathf.Clamp01(textTimer / textFadeInDuration);
                float textAlpha = Mathf.SmoothStep(0f, 1f, textT);
                SetAlpha(tipText, textAlpha);
            }

            yield return null;
        }

        SetAlpha(targetImage, 1f);
        SetAlpha(tipText, 1f);
    }

    public void HideImage()
    {
        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        if (tipCoroutine != null)
        {
            StopCoroutine(tipCoroutine);
            tipCoroutine = null;
        }

        fadeCoroutine = StartCoroutine(FadeOutImageAndText());
    }

    private IEnumerator FadeOutImageAndText()
    {
        float timer = 0f;

        float imageStartAlpha = targetImage != null ? targetImage.color.a : 0f;
        float textStartAlpha = tipText != null ? tipText.color.a : 0f;

        while (timer < fadeOutDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / fadeOutDuration);

            SetAlpha(targetImage, Mathf.Lerp(imageStartAlpha, 0f, t));
            SetAlpha(tipText, Mathf.Lerp(textStartAlpha, 0f, t));

            yield return null;
        }

        SetAlpha(targetImage, 0f);
        SetAlpha(tipText, 0f);
    }

    private void HideImageImmediate()
    {
        SetAlpha(targetImage, 0f);
        SetAlpha(tipText, 0f);
    }

    private void SetAlpha(Graphic graphic, float alpha)
    {
        if (graphic == null) return;

        Color c = graphic.color;
        c.a = alpha;
        graphic.color = c;
    }
}