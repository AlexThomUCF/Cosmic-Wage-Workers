using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnMainMenu : MonoBehaviour
{
    [SerializeField] private string mainMenuScene = "MainScene";

    public void ReturnToMainMenu()
    {
        if (string.IsNullOrEmpty(mainMenuScene))
        {
            Debug.LogError("Main menu scene name is not set!");
            return;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;


        // Load the main menu scene directly
        SceneManager.LoadScene(mainMenuScene);
    }
}