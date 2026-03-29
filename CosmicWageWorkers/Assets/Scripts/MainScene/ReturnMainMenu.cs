using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnMainMenu : MonoBehaviour
{
    [SerializeField] private string mainMenuScene = "UI";

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
}