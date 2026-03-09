using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FPSManager : MonoBehaviour
{
    public static FPSManager Instance { get; private set; }

    [SerializeField] SceneLoader loader;
    [SerializeField] PropManager spawner;

    [Header("Win Settings")]
    public int killCount = 0;
    public int killsToWin = 10;
    public string nextSceneName = "ProgramPrototype";

    [Header("Lose Settings")]
    public int enemiesEscaped = 0;
    public int maxEscapesAllowed = 5;
    public string loseSceneName = "GameOver";

    [Header("Customer Interaction ID")]
    public string interactionID;

    [Header("UI")]
    public TextMeshProUGUI killText;

    private bool gameEnded = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        loader = FindFirstObjectByType<SceneLoader>();

        killText.text = (killCount.ToString() + " / " + killsToWin.ToString());
        

    }

    // ======================
    // CORE GAME EVENTS
    // ======================

    public void RegisterKill()
    {
        if (gameEnded) return;

        killCount++;
        killText.text = (killCount.ToString() + " / " + killsToWin.ToString());


        if (killCount >= killsToWin)
        {
            WinGame();
        }
    }

    public void RegisterEscape()
    {
        if (gameEnded) return;

        enemiesEscaped++;

        if (enemiesEscaped >= maxEscapesAllowed)
        {
            LoseGame();
        }
    }



    // ======================
    // END STATES
    // ======================

    void WinGame()
    {
        gameEnded = true;

        string currentScene = SceneManager.GetActiveScene().name;
        if (currentScene == "FPSMainScene")
        {
            FindFirstObjectByType<ReturnMainMenu>().ReturnToMainMenu();
            return;
        }

        if (!string.IsNullOrEmpty(interactionID))
        {
            CustomerManager.MarkInteractionComplete(interactionID);
        }

        FinalMiniGame.miniGameCount++;
        loader.LoadSceneByName(nextSceneName);
    }

    void LoseGame()
    {
        gameEnded = true;
        loader.LoadSceneByName(loseSceneName);
    }
}
