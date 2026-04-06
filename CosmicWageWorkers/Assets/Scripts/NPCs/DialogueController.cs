using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System.Collections;

public class DialogueController : MonoBehaviour
{
    public static DialogueController Instance { get; private set; }

    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Image portraitImage;

    public Transform[] choicePanels;
    public GameObject choiceButtonPrefab;
    public Button closeButton;

    [Header("Typing Settings")]
    public float typingSpeed = 0.03f;

    private Coroutine typingCoroutine;
    private bool isTyping = false;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        dialoguePanel.SetActive(false);
    }

    // =============================
    // MAIN FUNCTION USED BY EVENTS
    // =============================
    public void ShowDialogue(string speaker, string text)
    {
        ShowDialogueUI(true);
        nameText.text = speaker;

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

    // =============================
    // TYPEWRITER EFFECT
    // =============================
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
    }

    // =============================
    // YOUR ORIGINAL SYSTEM
    // =============================
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

        foreach (Transform child in choicePanels[panelIndex])
            Destroy(child.gameObject);

        GameObject choiceButton = Instantiate(choiceButtonPrefab, choicePanels[panelIndex]);

        choiceButton.GetComponentInChildren<TMP_Text>().text = choiceText;
        choiceButton.GetComponent<Button>().onClick.AddListener(onClick);
    }
}