using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class TtleScreen : MonoBehaviour
{
    [SerializeField] UIDocument titleDocument;
    private VisualElement root;

    public GameObject titleScreenCamera;


    private float settingsUpDelayed = 1;

    private float settingsDownDelayed = 1;

    private float gameStaredDelayed = 2;

    public bool settingsOn;

    public bool settingsOff;

    public Animator cameraAnimation;

    public bool gameHasStarted;

    private AudioManager audioManager;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioManager = GameObject.Find("AudioManager").GetComponent<AudioManager>();
        root = titleDocument.rootVisualElement;

        var playBn = root.Q<VisualElement>("PlayBn");
        playBn.RegisterCallback<ClickEvent>(PlayGameClick);

        var settingsBn = root.Q<VisualElement>("SettingsBn");
        settingsBn.RegisterCallback<ClickEvent>(ToggleSettingsClick);

        var quitBn = root.Q<VisualElement>("QuitBn");
        quitBn.RegisterCallback<ClickEvent>(QuitGameClick);

        var backBn = root.Q<VisualElement>("BackBn");
        backBn.RegisterCallback<ClickEvent>(BackToMainClick);

        var graphicsDropDown= root.Q<DropdownField>("Graphics");
        List<string> qualityLevels = new List<string>(QualitySettings.names);
        graphicsDropDown.choices = qualityLevels;

        graphicsDropDown.index = QualitySettings.GetQualityLevel();
        graphicsDropDown.RegisterValueChangedCallback(evt =>
        {
            int selectedIndex = graphicsDropDown.index;
            QualitySettings.SetQualityLevel(selectedIndex, true);
            Debug.Log("You switch quality level to:   " + QualitySettings.names[selectedIndex]);
        });


        var vsyncBn = root.Q<RadioButtonGroup>("Vsync");
        vsyncBn.choices = new List<string> { "On", "Off" };

        var audioBn = root.Q<VisualElement>("AudioBn");
        audioBn.RegisterCallback<ClickEvent>(OpenAudio);

        var displayBn = root.Q<VisualElement>("DisplayBn");
        displayBn.RegisterCallback<ClickEvent>(OpenDisplay);

        var controlsBn = root.Q<VisualElement>("ControlsBn");
        controlsBn.RegisterCallback<ClickEvent>(OpenControls);

        audioManager.PlayVoice(audioManager.helloThere);
 
    }

    // Update is called once per frame
    void Update()
    {
         if (settingsOn)
        {
            var mmPanel = root.Q<VisualElement>("MainMenuBn");
            mmPanel.style.display = DisplayStyle.None;

            var titleMainMenu = root.Q<VisualElement>("Title");
            titleMainMenu.style.display = DisplayStyle.None;

            settingsUpDelayed -= Time.deltaTime;
            if (settingsUpDelayed < 0)
            {
                var settingsPanel = root.Q<VisualElement>("SettingsPanel");
                settingsPanel.style.display = DisplayStyle.Flex;

                settingsUpDelayed = 1;
                settingsOn = false;
            }

        }
        if (settingsOff)
        {

            var settingsPanel = root.Q<VisualElement>("SettingsPanel");
            settingsPanel.style.display = DisplayStyle.None;

            settingsDownDelayed -= Time.deltaTime;
            if (settingsDownDelayed < 0)
            {
                var mmPanel = root.Q<VisualElement>("MainMenuBn");
                mmPanel.style.display = DisplayStyle.Flex;

                var titleMainMenu = root.Q<VisualElement>("Title");
                titleMainMenu.style.display = DisplayStyle.Flex;

                settingsDownDelayed = 1;
                settingsOff = false;

            }
        }

        if (gameHasStarted)
        {
            var mmPanel = root.Q<VisualElement>("MainMenuBn");
            mmPanel.style.display = DisplayStyle.None;

            var titleMainMenu = root.Q<VisualElement>("Title");
            titleMainMenu.style.display = DisplayStyle.None;

            gameStaredDelayed -= Time.deltaTime;
            if (gameStaredDelayed < 0)
            {
                SceneManager.LoadScene("ProgramPrototype");
            }
        }
    }

    private void PlayGameClick(ClickEvent evt)
    {
        Debug.Log("GameHasStarted");
        gameHasStarted = true;
        cameraAnimation.SetTrigger("GameStarted");

        audioManager.PlayVoice(audioManager.helloThere);
        audioManager.PlaySFX(audioManager.buttonPress);
        
    }

    private void ToggleSettingsClick(ClickEvent evt)
    {
        Debug.Log("SettingsAreOpened");
        settingsOn = true;
        cameraAnimation.SetTrigger("SettingsUp");

        audioManager.PlayVoice(audioManager.helloThere);
        audioManager.PlaySFX(audioManager.buttonPress);

    }

    private void QuitGameClick(ClickEvent evt)
    {
        Debug.Log("GameHasStopped");
        Application.Quit();
    }

    private void BackToMainClick(ClickEvent evt)
    {
        Debug.Log("SettingsAreClosed");

        audioManager.PlayVoice(audioManager.helloThere);
        audioManager.PlaySFX(audioManager.buttonPress);

        settingsOff = true;
        cameraAnimation.SetTrigger("SettingsDown");
    }

    private void OpenAudio(ClickEvent evt)
    {
        audioManager.PlaySFX(audioManager.buttonPress);
        audioManager.PlayVoice(audioManager.helloThere);


        var optionsPanel = root.Q<VisualElement>("OptionsPanel");
        optionsPanel.style.display = DisplayStyle.Flex;

        var audioPanel = root.Q<VisualElement>("Audio");
        audioPanel.style.display = DisplayStyle.Flex;

        var displayPanel = root.Q<VisualElement>("Display");
        displayPanel.style.display = DisplayStyle.None;

        var controlsPanel = root.Q<VisualElement>("Controls");
        controlsPanel.style.display = DisplayStyle.None;

    }

    private void OpenDisplay(ClickEvent evt)
    {
        audioManager.PlaySFX(audioManager.buttonPress);
        audioManager.PlayVoice(audioManager.helloThere);

        var optionsPanel = root.Q<VisualElement>("OptionsPanel");
        optionsPanel.style.display = DisplayStyle.Flex;

        var audioPanel = root.Q<VisualElement>("Audio");
        audioPanel.style.display = DisplayStyle.None;

        var displayPanel = root.Q<VisualElement>("Display");
        displayPanel.style.display = DisplayStyle.Flex;

        var controlsPanel = root.Q<VisualElement>("Controls");
        controlsPanel.style.display = DisplayStyle.None;
    }

    private void OpenControls(ClickEvent evt)
    {
        audioManager.PlaySFX(audioManager.buttonPress);
        audioManager.PlayVoice(audioManager.helloThere);

        var optionsPanel = root.Q<VisualElement>("OptionsPanel");
        optionsPanel.style.display = DisplayStyle.Flex;

        var audioPanel = root.Q<VisualElement>("Audio");
        audioPanel.style.display = DisplayStyle.None;

        var displayPanel = root.Q<VisualElement>("Display");
        displayPanel.style.display = DisplayStyle.None;

        var controlsPanel = root.Q<VisualElement>("Controls");
        controlsPanel.style.display = DisplayStyle.Flex;
    }



}
