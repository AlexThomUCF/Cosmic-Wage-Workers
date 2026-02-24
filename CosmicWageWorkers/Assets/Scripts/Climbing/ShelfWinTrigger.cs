using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class ShelfWinTrigger : MonoBehaviour
{
    [Header("Customer Interaction ID")]
    public string interactionID;

    [Header("References")]
    public GameObject grabItem;
    public CanvasGroup promptUI;
    public CanvasGroup fadeCanvas;

    [Header("Detection")]
    public LayerMask playerLayer;

    [Header("Settings")]
    public float fadeDuration = 2f;
    public string nextSceneName = "MainScene";

    private bool playerInside;
    private bool hasGrabbed;
    private BoxCollider box;

    void Start()
    {
        box = GetComponent<BoxCollider>();

        promptUI.alpha = 0f;
        promptUI.interactable = false;
        promptUI.blocksRaycasts = false;

        fadeCanvas.alpha = 0f;
    }

    void Update()
    {
        CheckPlayerOverlap();

        if (!playerInside || hasGrabbed)
            return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(GrabAndFinish());
        }
    }

    void CheckPlayerOverlap()
    {
        Vector3 center = box.bounds.center;
        Vector3 halfExtents = box.bounds.extents;

        bool hit = Physics.CheckBox(
            center,
            halfExtents,
            transform.rotation,
            playerLayer
        );

        if (hit && !playerInside)
            OnPlayerEnter();

        if (!hit && playerInside)
            OnPlayerExit();
    }

    void OnPlayerEnter()
    {
        playerInside = true;

        HandStamina stamina = FindObjectOfType<HandStamina>();
        if (stamina != null)
            stamina.stopStamina = true;

        promptUI.alpha = 1f;
        promptUI.interactable = true;
        promptUI.blocksRaycasts = true;
    }

    void OnPlayerExit()
    {
        playerInside = false;

        promptUI.alpha = 0f;
        promptUI.interactable = false;
        promptUI.blocksRaycasts = false;
    }

    IEnumerator GrabAndFinish()
    {
        if (hasGrabbed)
            yield break;

        hasGrabbed = true;

        promptUI.alpha = 0f;
        grabItem.SetActive(false);

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fadeCanvas.alpha = elapsed / fadeDuration;
            yield return null;
        }

        if (!string.IsNullOrEmpty(interactionID))
            CustomerManager.MarkInteractionComplete(interactionID);

        FinalMiniGame.miniGameCount++;
        SaveSystem.SaveGame();

        fadeCanvas.alpha = 1f;
        SceneManager.LoadScene(nextSceneName);
    }
}