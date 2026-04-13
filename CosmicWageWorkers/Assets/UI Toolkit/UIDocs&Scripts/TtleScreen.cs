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
    public GameObject creditsScreen;

    public GameObject audioSettings;
    public GameObject displaySettings;
    public GameObject controlSettings;
    public GameObject keyboardControlsPanel;
    public GameObject gamepadControlsPanel;
    public GameObject backButton;

    [Header("First Selected Options")]
    [SerializeField] private GameObject _mainMenuFirst;
    [SerializeField] private GameObject _settingsMenuFirst;
    [SerializeField] private GameObject _playMenuFirst;
    [SerializeField] private GameObject _creditsMenuFirst;

    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        audioManager.PlayVoice(audioManager.helloThere);
        EventSystem.current.SetSelectedGameObject(_mainMenuFirst);
    }

    void Update()
    {
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

    public void PlayGameClick()
    {
        Debug.Log("Open Load Menu");
        Debug.Log(Application.persistentDataPath);

        audioManager.PlayVoice(audioManager.helloThere);
        audioManager.PlaySFX(audioManager.buttonPress);

        titleScreen.SetActive(false);
        loadGameMenu.SetActive(true);

        EventSystem.current.SetSelectedGameObject(_playMenuFirst);
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
        CustomerManager.SetCompletedInteractions(new List<string>());

        loadGameMenu.SetActive(false);
        gameHasStarted = true;
        cameraAnimation.SetTrigger("GameStarted");
    }

    public void ToggleSettingsClick()
    {
        settingsOn = true;
        cameraAnimation.SetTrigger("SettingsUp");

        audioManager.PlayVoice(audioManager.helloThere);
        audioManager.PlaySFX(audioManager.buttonPress);
    }

    public void BackToMainClick()
    {
        audioManager.PlayVoice(audioManager.helloThere);
        audioManager.PlaySFX(audioManager.buttonPress);

        audioSettings.SetActive(false);
        displaySettings.SetActive(false);
        controlSettings.SetActive(false);
        keyboardControlsPanel.SetActive(false);
        gamepadControlsPanel.SetActive(false);

        settingsOff = true;
        cameraAnimation.SetTrigger("SettingsDown");
    }

    public void OpenAudio()
    {
        audioManager.PlaySFX(audioManager.buttonPress);
        audioManager.PlayVoice(audioManager.helloThere);

        audioSettings.SetActive(true);
        displaySettings.SetActive(false);
        controlSettings.SetActive(false);
        keyboardControlsPanel.SetActive(false);
        gamepadControlsPanel.SetActive(false);
    }

    public void OpenDisplay()
    {
        audioManager.PlaySFX(audioManager.buttonPress);
        audioManager.PlayVoice(audioManager.helloThere);

        audioSettings.SetActive(false);
        displaySettings.SetActive(true);
        controlSettings.SetActive(false);
        keyboardControlsPanel.SetActive(false);
        gamepadControlsPanel.SetActive(false);
    }

    public void OpenControls()
    {
        audioManager.PlaySFX(audioManager.buttonPress);
        audioManager.PlayVoice(audioManager.helloThere);

        audioSettings.SetActive(false);
        displaySettings.SetActive(false);
        controlSettings.SetActive(true);
    }

    public void OpenKeyboardControls()
    {
        audioManager.PlaySFX(audioManager.buttonPress);
        audioManager.PlayVoice(audioManager.helloThere);

        audioSettings.SetActive(false);
        displaySettings.SetActive(false);

        keyboardControlsPanel.SetActive(true);
        gamepadControlsPanel.SetActive(false);
    }

    public void OpenGamepadControls()
    {
        audioManager.PlaySFX(audioManager.buttonPress);
        audioManager.PlayVoice(audioManager.helloThere);

        audioSettings.SetActive(false);
        displaySettings.SetActive(false);

        keyboardControlsPanel.SetActive(false);
        gamepadControlsPanel.SetActive(true);
    }

    public void OpenCreditsClick()
    {
        audioManager.PlayVoice(audioManager.helloThere);
        audioManager.PlaySFX(audioManager.buttonPress);

        titleScreen.SetActive(false);
        creditsScreen.SetActive(true);

        EventSystem.current.SetSelectedGameObject(_creditsMenuFirst);

        cameraAnimation.SetTrigger("CreditsOpen");
    }

    public void BackFromCreditsClick()
    {
        audioManager.PlayVoice(audioManager.helloThere);
        audioManager.PlaySFX(audioManager.buttonPress);

        creditsScreen.SetActive(false);
        titleScreen.SetActive(true);

        EventSystem.current.SetSelectedGameObject(_mainMenuFirst);

        cameraAnimation.SetTrigger("CreditsClose");
    }

    public void CloseCreditsNoAnimation()
    {
        audioManager.PlaySFX(audioManager.buttonPress);

        creditsScreen.SetActive(false);
        titleScreen.SetActive(true);

        EventSystem.current.SetSelectedGameObject(_mainMenuFirst);
    }

    public void QuitGameClick()
    {
        Application.Quit();
    }
}