using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteraction
{
    public UnityEvent onInteract { get; set; } = new UnityEvent();

    private DialogueController dialogueUI;
    public NPCDialogue dialogueData;
    private int dialogueIndex;
    private bool isTyping, isDialogueActive;
    public static bool isInDialogue = false;

    public void Start()
    {
        dialogueUI = DialogueController.Instance;
    }
    public void Interact()
    {
        if(dialogueData == null)
        {
            return;
        }

        onInteract?.Invoke();
        SoundEffectManager.Play("Interact");
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (isDialogueActive)
        {
            NextLine();
        }
        else
        {
            StartDialogue();
        }
    }

    void StartDialogue()
    { 
      isDialogueActive = true;
      isInDialogue = true;
      dialogueIndex = 0;


        dialogueUI.SetNPCInfo(dialogueData.npcName, dialogueData.npcProtrait);
        dialogueUI.ShowDialogueUI(true);
      
        //Pauce controller? if you want game to pause when interacting with dialogue, PauseController.SetPause(true);

        DisplayCurrentLine();


    }
    void NextLine()
    {
        if(isTyping)
        {
            StopAllCoroutines();
            dialogueUI.SetDialogueText(dialogueData.dialogueLines[dialogueIndex]);

            isTyping = false;
        }

        //Clear choices
        dialogueUI.ClearChoices();

        //Check endDialogueLines
        if(dialogueData.endDialogueLines.Length > dialogueIndex && dialogueData.endDialogueLines[dialogueIndex])
        {
            EndDialogue();
            return;
        }

        //Check if choices & display 
        foreach(DialogueChoice dialogueChoice in dialogueData.choices)
        {
            if(dialogueChoice.dialogueIndex == dialogueIndex)
            {
                DisplayChoices(dialogueChoice);
                return;
            }
        }

        if(++dialogueIndex < dialogueData.dialogueLines.Length)
        {
            DisplayCurrentLine();
        }
        else
        {
            EndDialogue();
        }

    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueUI.SetDialogueText("");

        foreach (char letter in dialogueData.dialogueLines[dialogueIndex]) // go through each letter in dialogueLine and add it to dialoguetext
        {
            dialogueUI.SetDialogueText(dialogueUI.dialogueText.text += letter);

            SoundEffectManager.PlayVoice(dialogueData.voiceSound, dialogueData.voicePitch);
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;

        if(dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex])
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
            int capturedIndex = i;

            UnityAction action = () =>
            {
                // Find a matching ChoiceEvent in the NPC
                foreach (var ce in choiceEvents)
                {
                    if (ce.choiceText.Equals(choiceText, System.StringComparison.OrdinalIgnoreCase))
                        ce.onChosen?.Invoke();
                }

                ChooseOption(nextIndex);
            };

            dialogueUI.CreateChoiceButton(choiceText, action);
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
        Cursor.lockState = CursorLockMode.Locked; // Locks cursor to center
        Cursor.visible = false;

        //pauseccontroller un pause game here
    }

    [System.Serializable]
    public class ChoiceEvent
    {
        public string choiceText;
        public UnityEvent onChosen;
    }

    public ChoiceEvent[] choiceEvents;
}
