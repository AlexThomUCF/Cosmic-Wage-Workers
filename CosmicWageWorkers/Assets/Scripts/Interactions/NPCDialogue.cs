using UnityEditor.U2D;
using UnityEngine;

[CreateAssetMenu(fileName = "NPCDialogue", menuName = "Scriptable Objects/NPCDialogue")]
public class NPCDialogue : ScriptableObject
{
    public string npcName;
    public Sprite npcProtrait;
    public string[] dialogueLines;
    public bool[] autoProgressLines;// will next npc line autoprogress without playing clicking 
    public float autoProgressDelay = 1.5f;// if npc stop talking, after this amount of time text will progress
    public float typingSpeed = 0.0f;
    public AudioClip voiceSound; //maybe wont need
    public float voicePitch = 1f; // maybe wont need

}
