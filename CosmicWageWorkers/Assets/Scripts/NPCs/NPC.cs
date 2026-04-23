using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Unity.Cinemachine;

public class NPC : MonoBehaviour, IInteraction
{
    public UnityEvent onInteract { get; set; } = new UnityEvent();

    private DialogueController dialogueUI;
    public NPCDialogue dialogueData;
    public SceneLoader loader;
    private int dialogueIndex;
    private bool isTyping, isDialogueActive;
    public static bool isInDialogue = false;

    public CinemachineCamera normalCam;
    public CinemachineCamera dialogueCam;
    private Vector3 originalCameraPosition;
    public Transform dialogueCameraPos;

    public CosmicPhenomenonManager cosmicManager;

    public void Start()
    {
        dialogueUI = DialogueController.Instance;
        cosmicManager = FindAnyObjectByType<CosmicPhenomenonManager>();

        GameObject camObj = GameObject.FindWithTag("dialogueCam");
        if (camObj != null) dialogueCam = camObj.GetComponent<CinemachineCamera>();

        GameObject normalObj = GameObject.FindWithTag("NormalCam");
        if (normalObj != null) normalCam = normalObj.GetComponent<CinemachineCamera>();

        GameObject loaderObj = GameObject.Find("SceneManager");
        if (loaderObj != null)
            loader = loaderObj.GetComponent<SceneLoader>();
        //loader = FindAnyObjectByType<SceneLoader>();
    }

    public void Interact()
    {
        if (dialogueData == null) return;

        onInteract?.Invoke();
        SoundEffectManager.Play("Interact");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (cosmicManager != null)
            cosmicManager.isPaused = true;

        if (isDialogueActive) NextLine();
        else StartDialogue();

        dialogueUI.SetCloseButton(this.EndDialogue);
        if (LoadingImageController.Instance != null)
        {
            LoadingImageController.Instance.SetSprite(dialogueData.loadingScreen);
        }
    }

    private void OnEnable()
    {
        StartCoroutine(RegisterWhenReady());
    }

    private IEnumerator RegisterWhenReady()
    {
        AssignExit assignExit = null;
        while (assignExit == null)
        {
            assignExit = FindAnyObjectByType<AssignExit>();
            yield return null;
        }
        assignExit.RegisterNPC(this);
    }

    void StartDialogue()
    {
        isDialogueActive = true;
        isInDialogue = true;
        dialogueIndex = 0;

        dialogueUI.SetNPCInfo(dialogueData.npcName, dialogueData.npcProtrait);
        dialogueUI.ShowDialogueUI(true);

        dialogueCam.transform.position = dialogueCameraPos.position;
        dialogueCam.transform.rotation = dialogueCameraPos.rotation;

        dialogueCam.Priority = 20;
        normalCam.Priority = 10;

        DisplayCurrentLine(); // show first line right away
    }

    void NextLine()
    {
        if (isTyping)
        {
            StopAllCoroutines();
            dialogueUI.SetDialogueText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
        }

        dialogueUI.ClearChoices();

        if (dialogueData.endDialogueLines.Length > dialogueIndex && dialogueData.endDialogueLines[dialogueIndex])
        {
            EndDialogue();
            return;
        }

        foreach (DialogueChoice dialogueChoice in dialogueData.choices)
        {
            if (dialogueChoice.dialogueIndex == dialogueIndex)
            {
                DisplayChoices(dialogueChoice);
                return;
            }
        }

        if (++dialogueIndex < dialogueData.dialogueLines.Length)
            DisplayCurrentLine();
        else EndDialogue();
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueUI.SetDialogueText("");

        foreach (char letter in dialogueData.dialogueLines[dialogueIndex])
        {
            dialogueUI.SetDialogueText(dialogueUI.dialogueText.text += letter);

            if (!SceneLoader.isLoading)
                SoundEffectManager.PlayVoice(dialogueData.voiceSound, dialogueData.voicePitch);

            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;

        if (dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            NextLine();
        }
    }

    void DisplayChoices(DialogueChoice choice)
    {
        for (int i = 0; i < choice.choices.Length; i++)
        {
            string choiceText = choice.choices[i];
            int nextIndex = choice.nextDialogueIndexs[i];

            UnityAction action = () =>
            {
                foreach (var ce in choiceEvents)
                {
                    if (ce.choiceText.Equals(choiceText, System.StringComparison.OrdinalIgnoreCase))
                        ce.onChosen?.Invoke();
                }

                ChooseOption(nextIndex);
            };

            dialogueUI.CreateChoiceButton(choiceText, action, i);
        }
    }

    void ChooseOption(int nextIndex)
    {
        dialogueIndex = nextIndex;
        dialogueUI.ClearChoices();
        DisplayCurrentLine();
    }

    void DisplayCurrentLine()
    {
        StopAllCoroutines();
        StartCoroutine(TypeLine());
    }

    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        isInDialogue = false;
        dialogueUI.SetDialogueText("");
        dialogueUI.ShowDialogueUI(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        dialogueCam.Priority = 0;
        normalCam.Priority = 20;

        if (cosmicManager != null)
            cosmicManager.isPaused = false;
    }

    public void StartDialogueExternally()
    {
        Interact();
    }   

    [System.Serializable]
    public class ChoiceEvent
    {
        public string choiceText;
        public UnityEvent onChosen;
    }

    public ChoiceEvent[] choiceEvents;
}