using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueController : MonoBehaviour
{
    public static DialogueController Instance { get; private set; }//Singleton instance
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Image portraitImage;
    public Transform choiceContainer;
    public GameObject choiceButtonPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
       if(Instance == null)
        {
            Instance = this;
        }
       else
        {
            Destroy(gameObject);
        }
    }

   public void ShowDialogueUI (bool show)
    {
        dialoguePanel.SetActive(show);//Toggle UI visability
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

    public void ClearChoices()
    {
        foreach(Transform child in choiceContainer) Destroy(child.gameObject);
    }

    public void  CreateChoiceButton(string choiceText, UnityEngine.Events.UnityAction onClick)
    {
        GameObject choiceButton = Instantiate(choiceButtonPrefab, choiceContainer);
        choiceButton.GetComponentInChildren<TMP_Text>().text = choiceText;
        choiceButton.GetComponent<Button>().onClick.AddListener(onClick);

    }

}
