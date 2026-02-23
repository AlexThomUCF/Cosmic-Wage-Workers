using UnityEngine;
using System.ComponentModel.Design.Serialization;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool gameIsPaused;
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject pauseButtons;
    public GameObject pauseStars;
    public GameObject generalHUD;
    public Animator pauseAni;
    public float pauseOnDelay = 0.3f;
    public float pauseDelay = 0.3f;
    public bool pauseLeave;
    public bool pauseOn;


 


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        //For pause animations to play
        if (pauseLeave)
        {
            pauseDelay -= Time.unscaledDeltaTime;
            
         
        }

        if (pauseOn)
        {
            pauseOnDelay -= Time.unscaledDeltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Escape) && !pauseOn)
        {
            PauseGame();
        }

        if (pauseDelay < 0)
        {
            pauseLeave = false;
            pauseMenu.SetActive(false);
            generalHUD.SetActive(true);
            pauseDelay = 0.3f;

}

        if (pauseOnDelay < 0)
        {
            pauseOn = false;
        }
    }

    //Function to pause the game
    public void PauseGame()
    {
        gameIsPaused = !gameIsPaused;

        if (gameIsPaused)
        {
            pauseMenu.SetActive(true);
            pauseAni.SetTrigger("PauseOn");
            generalHUD.SetActive(false);
            pauseOn = true;
            Time.timeScale = 0f;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
        }
        else
        {
            Time.timeScale = 1f;
            pauseAni.SetTrigger("PauseOff");
            pauseLeave = true;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        }
    
        
        }
    

    //Removes everything from pause to bring in options
    public void TurnOnOptions()
    {
        pauseButtons.SetActive(false);
        pauseStars.SetActive(false);
        optionsMenu.SetActive(true);
    }

    // Removes options and brings everything back
    public void TurnOffOptions()
    {
        pauseButtons.SetActive(true);
        pauseStars.SetActive(true);
        optionsMenu.SetActive(false);
    }

    //Return to title screen
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("UI");
    }
}
