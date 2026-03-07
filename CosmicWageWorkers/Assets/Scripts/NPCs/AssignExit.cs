using UnityEngine;
using UnityEngine.UI;

public class AssignExit : MonoBehaviour
{
    private Button button;
    private NPC currentNPC;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnExitPressed);
    }

    public void RegisterNPC(NPC npc)
    {
        currentNPC = npc;
        Debug.Log("NPC Registered: " + npc.name);
    }

    private void OnExitPressed()
    {
        if (currentNPC != null)
        {
            currentNPC.EndDialogue();
        }
        else
        {
            Debug.Log("No NPC registered.");
        }
    }

    public void ClearNPC()
    {
        currentNPC = null;
    }
}