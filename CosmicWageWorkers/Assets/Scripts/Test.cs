using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public NPCDialogue dialogueData;
    public UnityEvent yesClick;
    public UnityEvent onClick;

    public void Start()
    {
        dialogueData.onClicks[0] = yesClick;
        dialogueData.onClicks[1] = onClick;
    }
    public void ChangeSceneThing()
    {
        SceneManager.LoadScene(dialogueData.miniGameName);
    } 

    public void DoNothing()
    {
      
        Debug.Log("Nothing");
    }
}
