using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.EventSystems;

public class DialogueController : MonoBehaviour
{
    public static DialogueController Instance { get; private set; }

    [Header("UI References")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Image portraitImage;

    [Header("Choices")]
    public Transform[] choicePanels;
    public GameObject choiceButtonPrefab;
    public Button closeButton;

    [Header("Typing Settings")]
    public float typingSpeed = 0.03f;

    private Coroutine typingCoroutine;
    private bool isTyping = false;
    private bool hasSelectedFirstChoice = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        dialoguePanel.SetActive(false);
    }


    // MAIN FUNCTION FOR EVENTS
    public void ShowDialogue(string speaker, string text)
    {
        ShowDialogueUI(true);
        nameText.text = speaker;

        hasSelectedFirstChoice = false; // reset for new dialogue

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText(text));
    }

    public void HideDialogue()
    {
        ShowDialogueUI(false);
    }

    public bool IsFinishedTyping()
    {
        return !isTyping;
    }


    // TYPEWRITER EFFECT
    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char letter in text)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;

        // Optional: make sure controller selection is still valid after typing
        if (!hasSelectedFirstChoice && choicePanels.Length > 0)
        {
            foreach (Transform panel in choicePanels)
            {
                if (panel.childCount > 0)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                    EventSystem.current.SetSelectedGameObject(panel.GetChild(0).gameObject);
                    hasSelectedFirstChoice = true;
                    break;
                }
            }
        }
    }


    // UI CONTROL
    public void ShowDialogueUI(bool show)
    {
        dialoguePanel.SetActive(show);
    }

    public void SetNPCInfo(string npcName, Sprite portrait)
    {
        nameText.text = npcName;
        portraitImage.sprite = portrait;
    }

    public void SetDialogueText(string text)
    {
        dialogueText.text = text;
    }

    public void SetCloseButton(UnityAction onClickAction)
    {
        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(onClickAction);
    }

    public bool IsDialoguePanelActive()
    {
        return dialoguePanel != null && dialoguePanel.activeInHierarchy;
    }

    // CHOICES
    public void ClearChoices()
    {
        foreach (Transform panel in choicePanels)
        {
            foreach (Transform child in panel)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public void CreateChoiceButton(string choiceText, UnityAction onClick, int panelIndex)
    {
        if (panelIndex < 0 || panelIndex >= choicePanels.Length)
        {
            Debug.LogWarning("Invalid panel index!");
            return;
        }

        // Remove old buttons in this panel
        foreach (Transform child in choicePanels[panelIndex])
            Destroy(child.gameObject);

        // Spawn new button
        GameObject choiceButton = Instantiate(choiceButtonPrefab, choicePanels[panelIndex]);
        choiceButton.GetComponentInChildren<TMP_Text>().text = choiceText;
        choiceButton.GetComponent<Button>().onClick.AddListener(onClick);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(choiceButton);
    }
}