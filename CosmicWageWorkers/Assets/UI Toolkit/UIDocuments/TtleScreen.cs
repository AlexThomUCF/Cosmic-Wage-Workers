using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class TtleScreen : MonoBehaviour
{
    [SerializeField] UIDocument titleDocument;
    private VisualElement root;

    public GameObject titleScreenCamera;
    public GameObject settingsCamera;
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
         
    }

    private void PlayGameClick(ClickEvent evt)
    {
        Debug.Log("GameHasStarted");
        SceneManager.LoadScene("UI2");
    }

    private void ToggleSettingsClick(ClickEvent evt)
    {
        Debug.Log("SettingsAreOpened");

        var mmPanel = root.Q<VisualElement>("MainMenuBn");
        mmPanel.style.display = DisplayStyle.None;

        var titleMainMenu = root.Q<VisualElement>("Title");
        titleMainMenu.style.display = DisplayStyle.None;

        var settingsPanel = root.Q<VisualElement>("SettingsPanel");
        settingsPanel.style.display = DisplayStyle.Flex;

        settingsCamera.SetActive(true);
        titleScreenCamera.SetActive(false);
    }

    private void QuitGameClick(ClickEvent evt)
    {
        Debug.Log("GameHasStopped");
        Application.Quit();
    }

    private void BackToMainClick(ClickEvent evt)
    {
        Debug.Log("SettingsAreClosed");

        var mmPanel = root.Q<VisualElement>("MainMenuBn");
        mmPanel.style.display = DisplayStyle.Flex;

        var titleMainMenu = root.Q<VisualElement>("Title");
        titleMainMenu.style.display = DisplayStyle.Flex;

        var settingsPanel = root.Q<VisualElement>("SettingsPanel");
        settingsPanel.style.display = DisplayStyle.None;

        settingsCamera.SetActive(false);
        titleScreenCamera.SetActive(true);
    }

}
