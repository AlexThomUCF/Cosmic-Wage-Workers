using UnityEngine;

public class Ref : MonoBehaviour
{
    public Dialogue dialogue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //dialogue = GetComponent<Dialogue>();
    }

    public void npcInteracted()
    {
        dialogue.gameObject.SetActive(true);
        dialogue.runDialogue();
    }
}
