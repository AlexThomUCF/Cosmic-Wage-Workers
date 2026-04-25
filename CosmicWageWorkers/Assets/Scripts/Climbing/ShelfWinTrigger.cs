using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using System.Collections;

public class ShelfWinTrigger : MonoBehaviour
{
    [Header("Customer Interaction ID")]
    public string interactionID;

    [Header("References")]
    [SerializeField] SceneLoader loader;
    public GameObject grabItem;
    public CanvasGroup promptUI;
    public CanvasGroup fadeCanvas;

    [Header("Detection")]
    public LayerMask playerLayer;

    [Header("Settings")]
    public float fadeDuration = 2f;
    public string nextSceneName = "MainScene";

    [Header("Sound")]
    public AudioClip pickupClip;

    [Header("Input")]
    public InputActionReference GrabItemAction;

    private bool playerInside;
    private bool hasGrabbed;
    private BoxCollider box;

    private void Awake()
    {
        loader = FindAnyObjectByType<SceneLoader>();
    }

    void Start()
    {
        box = GetComponent<BoxCollider>();

        promptUI.alpha = 0f;
        promptUI.interactable = false;
        promptUI.blocksRaycasts = false;

        fadeCanvas.alpha = 0f;

        if (GrabItemAction != null)
            GrabItemAction.action.Enable();
    }

    private void OnEnable()
    {
        if (GrabItemAction != null)
            GrabItemAction.action.performed += OnGrabPerformed;
    }

    private void OnDisable()
    {
        if (GrabItemAction != null)
            GrabItemAction.action.performed -= OnGrabPerformed;
    }

    void Update()
    {
        CheckPlayerOverlap();
    }

    private void OnGrabPerformed(InputAction.CallbackContext context)
    {
        if (!playerInside || hasGrabbed)
            return;

        StartCoroutine(GrabAndFinish());
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

        Debug.Log("Player entered trigger");
    }

    void OnPlayerExit()
    {
        playerInside = false;

        promptUI.alpha = 0f;
        promptUI.interactable = false;
        promptUI.blocksRaycasts = false;

        Debug.Log("Player exited trigger");
    }

    IEnumerator GrabAndFinish()
    {
        if (hasGrabbed)
            yield break;

        hasGrabbed = true;

        Debug.Log("Grab triggered");

        if (pickupClip != null)
            AudioSource.PlayClipAtPoint(pickupClip, transform.position);

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
        fadeCanvas.gameObject.SetActive(false);

        Debug.Log("Loading next scene...");
        loader.LoadSceneByName(nextSceneName);
    }
}