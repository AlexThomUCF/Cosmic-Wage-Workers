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
    public GameObject pauseStars;
    public GameObject generalHUD;
    public Animator pauseAni;
    public float pauseOnDelay = 1f;
    public float pauseDelay = 0.5f;
    public bool pauseLeave;
    public bool pauseOn;
 


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        if (pauseLeave)
        {
            pauseDelay -= Time.deltaTime;
        }

        if (pauseOn)
        {
            pauseOnDelay -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();

        }

        if (pauseDelay < 0)
        {
            pauseMenu.SetActive(false);
            generalHUD.SetActive(true);
            pauseLeave = false;
            pauseDelay = 0.5f;     
        }

        if (pauseOnDelay < 0)
        {
            Time.timeScale = 0f;
            pauseOn = false;
            pauseOnDelay = 1f;
        }
    }

    public void PauseGame()
    {
        gameIsPaused = !gameIsPaused;

        if (gameIsPaused)
        {
            pauseMenu.SetActive(true);
            pauseAni.SetTrigger("PauseOn");
            generalHUD.SetActive(false);
            pauseOn = true;
        }
        else
        {
            Time.timeScale = 1f;
            pauseLeave = true;
            pauseAni.SetTrigger("PauseOff");
        
        }
    }

    public void TurnOnOptions()
    {
        pauseButtons.SetActive(false);
        pauseStars.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void TurnOffOptions()
    {
        pauseButtons.SetActive(true);
        pauseStars.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("UI");
    }
}
