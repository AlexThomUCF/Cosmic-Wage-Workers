using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PortalTrigger : MonoBehaviour
{
    [SerializeField] private GravityManager gravityManager;
    [SerializeField] private float fadeDuration = 2f;
    [SerializeField] private Color fadeColor = new Color(0.14f, 0.38f, 0.16f, 1f); // #24612A

    private CanvasGroup canvasGroup;
    private bool isFading = false;

    private void Start()
    {
        // Create a canvas for the fade effect if it doesn't exist
        if (canvasGroup == null)
        {
            GameObject fadePanel = new GameObject("FadePanel");
            RectTransform rectTransform = fadePanel.AddComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;

            Canvas canvas = fadePanel.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 999;

            Image image = fadePanel.AddComponent<Image>();
            image.color = new Color(fadeColor.r, fadeColor.g, fadeColor.b, 0f);

            canvasGroup = fadePanel.AddComponent<CanvasGroup>();
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player") && !isFading)
        {
            StartCoroutine(FadeAndLoad());
        }
    }

    private System.Collections.IEnumerator FadeAndLoad()
    {
        isFading = true;
        float elapsedTime = 0f;

        Image fadeImage = canvasGroup.GetComponent<Image>();

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            
            Color newColor = fadeImage.color;
            newColor.a = alpha;
            fadeImage.color = newColor;

            yield return null;
        }

        // Ensure fully opaque
        Color finalColor = fadeImage.color;
        finalColor.a = 1f;
        fadeImage.color = finalColor;

        // Reset gravity and load scene
        Physics.gravity = new Vector3(0, gravityManager.gravityScale, 0);

        if (!string.IsNullOrEmpty(gravityManager.interactionID))
        {
            CustomerManager.MarkInteractionComplete(gravityManager.interactionID);
        }

        FinalMiniGame.miniGameCount++;
        SaveSystem.SaveGame();

        SceneManager.LoadScene("MainScene");
    }
}
