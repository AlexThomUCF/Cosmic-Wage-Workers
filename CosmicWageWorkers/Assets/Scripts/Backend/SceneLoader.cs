using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] Animator transitonAnim;
    [SerializeField] GameObject transitonObj;
    [SerializeField] Canvas canvas;

    public void Awake()
    {
        transitonObj = GameObject.Find("Scene Tansition");
        transitonAnim = transitonObj.GetComponent<Animator>();
       canvas = transitonObj.GetComponentInChildren<Canvas>();
    }
    public void LoadSceneByName(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Scene name is empty!");
            return;
        }
        NPC.isInDialogue = false;
        StartCoroutine(LoadLevel(sceneName));
        
    }

    public void ExitDialogue()
    {
        if (DialogueController.Instance != null)
        {
            DialogueController.Instance?.ShowDialogueUI(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Debug.Log("Dialogue closed.");
        }
        else
            return;
    }
    public IEnumerator LoadLevel(string sceneName)
    {
        transitonAnim.SetTrigger("End");
        ExitDialogue();
        Debug.Log("Doing transition");
        Debug.Log("Animator reference: " + transitonAnim, transitonAnim);
        canvas.sortingOrder = 2;
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName);
        transitonAnim.SetTrigger("Start");
        canvas.sortingOrder = -1;
        //change scene
    }

    public void ExitMiniGame()
    {
        string mainScene = "MainScene";
        LoadLevel(mainScene);
    }
} 
