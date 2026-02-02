using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ShelfWinTrigger : MonoBehaviour
{
    [Header("References")]
    public GameObject grabItem;
    public CanvasGroup promptUI;
    public CanvasGroup fadeCanvas;

    [Header("Settings")]
    public float fadeDuration = 2f;
    public string nextSceneName = "MainScene";

    private bool playerInside;
    private bool hasGrabbed;

    void Start()
    {
        promptUI.alpha = 0f;
        promptUI.interactable = false;
        promptUI.blocksRaycasts = false;

        fadeCanvas.alpha = 0f;
    }

    void Update()
    {
        if (!playerInside || hasGrabbed)
            return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(GrabAndFinish());
        }
    }

    IEnumerator GrabAndFinish()
    {
        hasGrabbed = true;

        // Hide UI & item
        promptUI.alpha = 0f;
        grabItem.SetActive(false);

        // Fade to black
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fadeCanvas.alpha = elapsed / fadeDuration;
            yield return null;
        }

        fadeCanvas.alpha = 1f;

        SceneManager.LoadScene(nextSceneName);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = true;

        // Stop stamina when entering win trigger
        HandStamina stamina = other.GetComponent<HandStamina>();
        if (stamina != null)
        {
            stamina.stopStamina = true;
        }

        promptUI.alpha = 1f;
        promptUI.interactable = true;
        promptUI.blocksRaycasts = true;
    }

    void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerInside = false;

        promptUI.alpha = 0f;
        promptUI.interactable = false;
        promptUI.blocksRaycasts = false;
    }
}