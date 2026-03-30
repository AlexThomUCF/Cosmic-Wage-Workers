using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem; // <-- needed for InputActionReference
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

    [Header("Sound")]
    public AudioClip pickupClip; // drag your audio file here

    [Header("Input")]
    public InputActionReference GrabItemAction; // <-- Input System action

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

        // Enable the input action
        if (GrabItemAction != null) GrabItemAction.action.Enable();
    }

    void Update()
    {
        CheckPlayerOverlap();

        if (!playerInside || hasGrabbed)
            return;

        // Use new Input System "Use" action
        if (GrabItemAction != null && GrabItemAction.action.WasPerformedThisFrame())
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

        // Play pickup sound at the object's position
        if (pickupClip != null)
            AudioSource.PlayClipAtPoint(pickupClip, transform.position);

        // Hide prompt and grabbed object
        promptUI.alpha = 0f;
        grabItem.SetActive(false);

        // Fade out screen
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fadeCanvas.alpha = elapsed / fadeDuration;
            yield return null;
        }

        // Mark interaction complete
        if (!string.IsNullOrEmpty(interactionID))
            CustomerManager.MarkInteractionComplete(interactionID);

        FinalMiniGame.miniGameCount++;
        SaveSystem.SaveGame();

        fadeCanvas.alpha = 1f;
        SceneManager.LoadScene(nextSceneName);
    }
}