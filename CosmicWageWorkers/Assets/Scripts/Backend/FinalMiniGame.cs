using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalMiniGame : MonoBehaviour
{
    [SerializeField] private int numberOfMinigames = 0;

    private static FinalMiniGame Instance;
    public static int miniGameCount = 0;

    private bool invasionStarted = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SaveSystem.LoadGame();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "MainScene") return;

        invasionStarted = false;

        TryTriggerInvasion();
    }

    void TryTriggerInvasion()
    {
        if (invasionStarted) return;
        if (miniGameCount < numberOfMinigames) return;

        InvasionAlarm controller = FindFirstObjectByType<InvasionAlarm>();

        if (controller == null) return;

        invasionStarted = true;
        controller.StartInvasionSequence();
    }
}