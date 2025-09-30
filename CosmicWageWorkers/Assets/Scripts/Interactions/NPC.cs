using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NPC : MonoBehaviour, IInteraction
{
    public UnityEvent onInteract { get; set; } = new UnityEvent();

    public NPCDialogue dialogueData;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Image portraitImage;

    private int dialogueIndex;
    private bool isTyping, isDialogueActive;

    public void Interact()
    {
        if(dialogueData == null)
        {
            return;
        }

        onInteract?.Invoke();

        if(isDialogueActive)
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
      dialogueIndex = 0;

      nameText.SetText(dialogueData.npcName);
      portraitImage.sprite = dialogueData.npcProtrait;

      dialoguePanel.SetActive(true);
        //Pauce controller? if you want game to pause when interacting with dialogue, PauseController.SetPause(true);

        StartCoroutine(TypeLine());


    }
    void NextLine()
    {
        if(isTyping)
        {
            StopAllCoroutines();
            dialogueText.SetText(dialogueData.dialogueLines[dialogueIndex]);
            isTyping = false;
        }
        else if(++dialogueIndex < dialogueData.dialogueLines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }

    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.SetText("");

        foreach(char letter in dialogueData.dialogueLines[dialogueIndex]) // go through each letter in dialogueLine and add it to dialoguetext
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(dialogueData.typingSpeed);
        }

        isTyping = false;

        if(dialogueData.autoProgressLines.Length > dialogueIndex && dialogueData.autoProgressLines[dialogueIndex])
        {
            yield return new WaitForSeconds(dialogueData.autoProgressDelay);
            NextLine();
        }
    }
    
    public void EndDialogue()
    {
        StopAllCoroutines();
        isDialogueActive = false;
        dialogueText.SetText("");
        dialoguePanel.SetActive(false);
        //pauseccontroller un pause game here
    }    
}
