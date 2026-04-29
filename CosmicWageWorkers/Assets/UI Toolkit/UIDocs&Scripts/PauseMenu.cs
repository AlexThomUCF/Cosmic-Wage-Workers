using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class PauseMenu : MonoBehaviour
{
    [Header("Panels & Animator")]
    public Animator pauseAni;               // Animator on PausePanel
    public CanvasGroup pauseCanvasGroup;    // Main pause panel
    public CanvasGroup pauseButtons;        // Buttons group
    public CanvasGroup optionsPanel;        // Options panel
    public CanvasGroup controlsPanel;       // NEW: Controls panel

    [Header("Other")]
    public GameObject generalHUD;           // HUD
    public PlayableDirector[] directors;

    [Header("First Selected Options")]
    [SerializeField] private GameObject _pauseMenuFirst;
    [SerializeField] private GameObject _settingsMenuFirst;
    [SerializeField] private GameObject _controlsMenuFirst; // NEW

    private bool gameIsPaused = false;
    private bool isTransitioning = false;

    void Start()
    {
        Time.timeScale = 1f;

        directors = FindObjectsByType<PlayableDirector>(FindObjectsSortMode.None);

        optionsPanel.alpha = 0f;
        optionsPanel.interactable = false;
        optionsPanel.blocksRaycasts = false;

        controlsPanel.alpha = 0f;
        controlsPanel.interactable = false;
        controlsPanel.blocksRaycasts = false;

        
    }

    private bool AnyDirectorPlaying()
    {
        foreach (PlayableDirector director in directors)
        {
            if (director != null && director.state == PlayState.Playing)
                return true;
        }

        return false;
    }

    public void OnPause(InputValue value)
    {
        if (!isTransitioning && value.isPressed && !AnyDirectorPlaying())
        {
            if (gameIsPaused) StartUnpause();
            else StartPause();

            Debug.Log("Paused");
        }
    }

    void StartPause()
    {
        isTransitioning = true;
        gameIsPaused = true;

        pauseCanvasGroup.alpha = 1f;
        pauseCanvasGroup.interactable = true;
        pauseCanvasGroup.blocksRaycasts = true;

        pauseButtons.alpha = 1f;
        pauseButtons.interactable = true;
        pauseButtons.blocksRaycasts = true;

        optionsPanel.alpha = 0f;
        optionsPanel.interactable = false;
        optionsPanel.blocksRaycasts = false;

        controlsPanel.alpha = 0f;
        controlsPanel.interactable = false;
        controlsPanel.blocksRaycasts = false;

        generalHUD.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        pauseAni.SetTrigger("PauseOn");
        EventSystem.current.SetSelectedGameObject(_pauseMenuFirst);
    }

    void StartUnpause()
    {
        isTransitioning = true;
        pauseAni.SetTrigger("PauseOff");
    }

    public void OnPauseAnimationFinished()
    {
        Time.timeScale = 0f;
        isTransitioning = false;
    }

    public void OnUnpauseAnimationFinished()
    {
        Time.timeScale = 1f;

        pauseCanvasGroup.alpha = 0f;
        pauseCanvasGroup.interactable = false;
        pauseCanvasGroup.blocksRaycasts = false;

        generalHUD.SetActive(true);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        gameIsPaused = false;
        isTransitioning = false;
    }

    public void ResumeGame()
    {
        if (!isTransitioning && gameIsPaused) StartUnpause();
    }

    public void OpenOptions()
    {
        if (isTransitioning) return;

        pauseButtons.alpha = 0f;
        pauseButtons.interactable = false;
        pauseButtons.blocksRaycasts = false;

        controlsPanel.alpha = 0f;
        controlsPanel.interactable = false;
        controlsPanel.blocksRaycasts = false;

        optionsPanel.alpha = 1f;
        optionsPanel.interactable = true;
        optionsPanel.blocksRaycasts = true;

        EventSystem.current.SetSelectedGameObject(_settingsMenuFirst);
    }

    public void CloseOptions()
    {
        optionsPanel.alpha = 0f;
        optionsPanel.interactable = false;
        optionsPanel.blocksRaycasts = false;

        pauseButtons.alpha = 1f;
        pauseButtons.interactable = true;
        pauseButtons.blocksRaycasts = true;

        EventSystem.current.SetSelectedGameObject(_pauseMenuFirst);
    }

    // NEW: Open Controls panel
    public void OpenControls()
    {
        if (isTransitioning) return;

        pauseButtons.alpha = 0f;
        pauseButtons.interactable = false;
        pauseButtons.blocksRaycasts = false;

        optionsPanel.alpha = 0f;
        optionsPanel.interactable = false;
        optionsPanel.blocksRaycasts = false;

        controlsPanel.alpha = 1f;
        controlsPanel.interactable = true;
        controlsPanel.blocksRaycasts = true;

        EventSystem.current.SetSelectedGameObject(_controlsMenuFirst);
    }

    // NEW: Close Controls panel
    public void CloseControls()
    {
        controlsPanel.alpha = 0f;
        controlsPanel.interactable = false;
        controlsPanel.blocksRaycasts = false;

        pauseButtons.alpha = 1f;
        pauseButtons.interactable = true;
        pauseButtons.blocksRaycasts = true;

        EventSystem.current.SetSelectedGameObject(_pauseMenuFirst);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("UI");
    }

    public void ExitMiniGame()
    {
        if (SceneManager.GetActiveScene().name != "MainScene")
        {
            SceneManager.LoadScene("MainScene");
        }
    }
}