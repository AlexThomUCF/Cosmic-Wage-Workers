using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public NPCDialogue dialogueData;
    public UnityEvent yesClick;
    public UnityEvent onClick;

    public void ChangeSceneThing()
    {
        SceneManager.LoadScene(dialogueData.miniGameName);
    } 

    public void DoNothing()
    {
        Debug.Log("Nothing");
    }
}
