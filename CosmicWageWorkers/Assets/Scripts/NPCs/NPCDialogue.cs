using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "NPCDialogue", menuName = "Scriptable Objects/NPCDialogue")]
public class NPCDialogue : ScriptableObject
{
    public string npcName;
    public string miniGameName;
    public Sprite npcProtrait;
    public string[] dialogueLines;
    public Sprite loadingScreen;
    public string[] tips;

    [Tooltip("will next npc line autoprogress without playing clicking ")]
    public bool[] autoProgressLines; 
    [Tooltip("Mark where dialogue ends")]
    public bool[] endDialogueLines; 
    [Tooltip("if npc stop talking, after this amount of time text will progress")]
    public float autoProgressDelay = 1.5f;
    public float typingSpeed = 0.0f;
    public AudioClip voiceSound; //maybe wont need
    public float voicePitch = 1f; // maybe wont need

    public UnityEvent[] onClicks;

    public DialogueChoice[] choices;

}
[System.Serializable]
public class DialogueChoice
{
    [Tooltip("Dialogue line where choices appear")]
    public int dialogueIndex; 
    [Tooltip("Player response options")]
    public string[] choices;
    [Tooltip("Where choice leads")]
    public int[] nextDialogueIndexs;
}
