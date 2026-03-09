using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] Animator transitonAnim;
    [SerializeField] GameObject transitonObj;
    [SerializeField] public Canvas canvas;
    [SerializeField] public Image targetImage;
    NPCDialogue dialogueData;
    public static bool isLoading = false;

    public void Awake()
    {
        transitonObj = GameObject.Find("Scene Tansition");
        transitonAnim = transitonObj.GetComponent<Animator>();
        //canvas = dialogueData.loadingScreen.GetComponent<Canvas>();
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
        isLoading = true;

        transitonAnim.SetTrigger("End");
        ExitDialogue();
        Debug.Log("Doing transition");
        Debug.Log("Animator reference: " + transitonAnim, transitonAnim);
        canvas.sortingOrder = 2;
        yield return new WaitForSeconds(4); // Used to be 1 changed to 8, The longer wait for seconds the longer you wait on loading screen.
        SceneManager.LoadScene(sceneName);
        transitonAnim.SetTrigger("Start");
        canvas.sortingOrder = -1;

        isLoading = false;
        //change scene
    }

    public void ExitMiniGame()
    {
        string mainScene = "MainScene";
        StartCoroutine(LoadLevel(mainScene));
    }
} 
