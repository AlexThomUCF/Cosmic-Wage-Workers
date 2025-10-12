using System.ComponentModel.Design.Serialization;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public bool gameIsPaused;
    public GameObject pauseMenu;
    public GameObject optionsMenu;
    public GameObject pauseButtons;
    public GameObject managerInPause;
    public GameObject pauseStars;
 


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        gameIsPaused = !gameIsPaused;

        if (gameIsPaused)
        {
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
        }
    }

    public void TurnOnOptions()
    {
        pauseButtons.SetActive(false);
        managerInPause.SetActive(false);
        pauseStars.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void TurnOffOptions()
    {
        pauseButtons.SetActive(true);
        managerInPause.SetActive(true);
        pauseStars.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("UI");
    }
}
