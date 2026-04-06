using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class TitleScreen : MonoBehaviour
{
    public GameObject titleScreenCamera;
    private float settingsUpDelayed = 1;
    private float settingsDownDelayed = 1;
    private float gameStaredDelayed = 2;
    public bool settingsOn;
    public bool settingsOff;
    public Animator cameraAnimation;

    public bool gameHasStarted;
    private AudioManager audioManager;
    public GameObject titleScreen;
    public GameObject loadGameMenu;
    public GameObject settingsScreen;
    public GameObject audioSettings;
    public GameObject displaySettings;
    public GameObject controlSettings;
    public GameObject backButton;

    [Header("First Selected Options")]
    [SerializeField] private GameObject _mainMenuFirst;
    [SerializeField] private GameObject _settingsMenuFirst;
    [SerializeField] private GameObject _playMenuFirst;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        audioManager.PlayVoice(audioManager.helloThere);
        EventSystem.current.SetSelectedGameObject(_mainMenuFirst);
    }

    // Update is called once per frame
    void Update()
    {
        //To bring up settings
         if (settingsOn)
        {
            titleScreen.SetActive(false);

            settingsUpDelayed -= Time.deltaTime;
            if (settingsUpDelayed < 0)
            {

                settingsScreen.SetActive(true);
                settingsUpDelayed = 1;
                settingsOn = false;
            }

            EventSystem.current.SetSelectedGameObject(_settingsMenuFirst);
        }

         //To shut off settings
        if (settingsOff)
        {
            settingsScreen.SetActive(false);

            settingsDownDelayed -= Time.deltaTime;
            if (settingsDownDelayed < 0)
            {

                titleScreen.SetActive(true);
                settingsDownDelayed = 1;
                settingsOff = false;

            }

            EventSystem.current.SetSelectedGameObject(_mainMenuFirst);
        }
        //For starting game and making sure animation plays
        if (gameHasStarted)
        {

            titleScreen.SetActive(false);
            gameStaredDelayed -= Time.deltaTime;
            if (gameStaredDelayed < 0)
            {
                SceneManager.LoadScene("MainScene");
            }
        }
    }

    //Starts Game
    public void PlayGameClick()
    {
        Debug.Log("Open Load Menu");
        Debug.Log(Application.persistentDataPath);

        audioManager.PlayVoice(audioManager.helloThere);
        audioManager.PlaySFX(audioManager.buttonPress);

        // Hide main title buttons
        titleScreen.SetActive(false);

        // Show Continue / New Game menu
        loadGameMenu.SetActive(true);

        EventSystem.current.SetSelectedGameObject(_playMenuFirst);
    }

    //Goes into settings
    public void ToggleSettingsClick()
    {
        Debug.Log("SettingsAreOpened");
        settingsOn = true;
        cameraAnimation.SetTrigger("SettingsUp");

        audioManager.PlayVoice(audioManager.helloThere);
        audioManager.PlaySFX(audioManager.buttonPress);

    }

    //Quit button
    public void QuitGameClick()
    {
        Debug.Log("GameHasStopped");
        Application.Quit();
    }

    //Return to title if in settings
    public void BackToMainClick()
    {
        Debug.Log("SettingsAreClosed");

        audioManager.PlayVoice(audioManager.helloThere);
        audioManager.PlaySFX(audioManager.buttonPress);


        audioSettings.SetActive(false);

        controlSettings.SetActive(false);

        displaySettings.SetActive(false);

        settingsOff = true;
        cameraAnimation.SetTrigger("SettingsDown");
    }

    //Opens Audio Section
    public void OpenAudio()
    {
        audioManager.PlaySFX(audioManager.buttonPress);
        audioManager.PlayVoice(audioManager.helloThere);

        audioSettings.SetActive(true);

        controlSettings.SetActive(false);

        displaySettings.SetActive(false);


    }

    //Opens display settings
    public void OpenDisplay()
    {
        audioManager.PlaySFX(audioManager.buttonPress);
        audioManager.PlayVoice(audioManager.helloThere);

        audioSettings.SetActive(false);

        controlSettings.SetActive(false);

        displaySettings.SetActive(true);


    }

    //Opens Controls
    public void OpenControls()
    {
        audioManager.PlaySFX(audioManager.buttonPress);
        audioManager.PlayVoice(audioManager.helloThere);

        audioSettings.SetActive(false);
        controlSettings.SetActive(true);
        displaySettings.SetActive(false);


    }

    public void ContinueGameClick()
    {
        if (SaveSystem.SaveExists())
        {
            SaveSystem.LoadGame();
        }

        loadGameMenu.SetActive(false);
        gameHasStarted = true;
        cameraAnimation.SetTrigger("GameStarted");
    }

    public void NewGameClick()
    {
        SaveSystem.DeleteSave();

        PlayerPrefs.DeleteAll();

        FinalMiniGame.miniGameCount = 0;
        CustomerManager.SetCompletedInteractions(new System.Collections.Generic.List<string>());

        loadGameMenu.SetActive(false);
        gameHasStarted = true;
        cameraAnimation.SetTrigger("GameStarted");
    }



}
