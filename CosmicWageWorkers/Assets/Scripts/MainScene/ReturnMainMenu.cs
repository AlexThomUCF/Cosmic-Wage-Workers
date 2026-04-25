using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class ReturnMainMenu : MonoBehaviour
{
    [SerializeField] private string mainMenuScene = "UI";
    [SerializeField] private HumanFpsCineController cineController; 

    public void ReturnToMainMenu()
    {
        if (string.IsNullOrEmpty(mainMenuScene))
        {
            Debug.LogError("Main menu scene name is not set!");
            return;
        }

        // Unlock and show the cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene(mainMenuScene);
    }

    public void EndCinematic()
    {
        
        cineController.endCine.gameObject.SetActive(true);
        if (cineController.endCine.state != PlayState.Playing)
        {
            Debug.Log("Reached here");
            cineController.endCine.gameObject.SetActive(false);
            ReturnToMainMenu();
        }
    }
}