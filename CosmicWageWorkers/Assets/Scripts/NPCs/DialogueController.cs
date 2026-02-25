using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class DialogueController : MonoBehaviour
{
    public static DialogueController Instance { get; private set; }//Singleton instance
    public GameObject dialoguePanel;
    public TMP_Text dialogueText, nameText;
    public Image portraitImage;
    public Transform[] choicePanels; // Assign in Inspector
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
        foreach (Transform panel in choicePanels)
        {
            foreach (Transform child in panel)
            {
                Destroy(child.gameObject);
            }
        }
    }

    public void CreateChoiceButton(string choiceText, UnityEngine.Events.UnityAction onClick, int panelIndex)
    {
        if (panelIndex < 0 || panelIndex >= choicePanels.Length)
        {
            Debug.LogWarning("Invalid panel index!");
            return;
        }

        // Clear previous button in that panel (optional safety)
        foreach (Transform child in choicePanels[panelIndex])
            Destroy(child.gameObject);

        // Spawn button inside predefined panel
        GameObject choiceButton = Instantiate(choiceButtonPrefab, choicePanels[panelIndex]);

        choiceButton.GetComponentInChildren<TMP_Text>().text = choiceText;
        choiceButton.GetComponent<Button>().onClick.AddListener(onClick);
    }

}
