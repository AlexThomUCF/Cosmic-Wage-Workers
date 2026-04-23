using UnityEngine;

public class CinematicDialogue : MonoBehaviour
{
    public NPC npc;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable()
    {
        npc.StartDialogueExternally();
    }
}
