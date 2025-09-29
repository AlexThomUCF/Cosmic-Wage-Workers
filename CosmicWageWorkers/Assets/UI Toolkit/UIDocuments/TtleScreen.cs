using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class TtleScreen : MonoBehaviour
{
    [SerializeField] UIDocument titleDocument;
    private VisualElement root;

    public GameObject titleScreenCamera;

    private float settingsUpDelayed = 2;

    private float settingsDownDelayed = 2;

    private float gameStaredDelayed = 3;

    public bool settingsOn;

    public bool settingsOff;

    public Animator cameraAnimation;

    public bool gameHasStarted;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
         root = titleDocument.rootVisualElement;

        var playBn = root.Q<VisualElement>("PlayBn");
        playBn.RegisterCallback<ClickEvent>(PlayGameClick);

        var settingsBn = root.Q<VisualElement>("SettingsBn");
        settingsBn.RegisterCallback<ClickEvent>(ToggleSettingsClick);

        var quitBn = root.Q<VisualElement>("QuitBn");
        quitBn.RegisterCallback<ClickEvent>(QuitGameClick);

        var backBn = root.Q<VisualElement>("BackBn");
        backBn.RegisterCallback<ClickEvent>(BackToMainClick);
 
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

                settingsUpDelayed = 2;
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

                settingsDownDelayed = 2;
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
                SceneManager.LoadScene("UI2");
            }
        }
    }

    private void PlayGameClick(ClickEvent evt)
    {
        Debug.Log("GameHasStarted");
        gameHasStarted = true;
        cameraAnimation.SetTrigger("GameStarted");
        
    }

    private void ToggleSettingsClick(ClickEvent evt)
    {
        Debug.Log("SettingsAreOpened");

        settingsOn = true;
        cameraAnimation.SetTrigger("SettingsUp");
     
    }

    private void QuitGameClick(ClickEvent evt)
    {
        Debug.Log("GameHasStopped");
        Application.Quit();
    }

    private void BackToMainClick(ClickEvent evt)
    {
        Debug.Log("SettingsAreClosed");

        settingsOff = true;
        cameraAnimation.SetTrigger("SettingsDown");
    }



}
