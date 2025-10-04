using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    public NPCDialogue dialogueData;
  
    public void ChangeSceneThing()
    {
        SceneManager.LoadScene(dialogueData.miniGameName);
    }
}
