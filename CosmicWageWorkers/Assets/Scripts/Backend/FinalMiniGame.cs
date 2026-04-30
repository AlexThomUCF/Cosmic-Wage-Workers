using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalMiniGame : MonoBehaviour
{
    [SerializeField] private int numberOfMinigames = 0;

    private static FinalMiniGame Instance;
    private SceneLoader loader;
    public static int miniGameCount = 0;

    private bool invasionStarted = false; // prevents multiple coroutines

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            SaveSystem.LoadGame(); // Ensure data loads
        }
        else
        {
            Destroy(gameObject);
        } 
        loader = FindFirstObjectByType<SceneLoader>();
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
        if (scene.name == "MainScene" && !invasionStarted && miniGameCount >= numberOfMinigames)
        {
            invasionStarted = true;
            StartCoroutine(InvasionComing());
        }
    }

    private void Update()
    {
        Debug.Log("Minigame count = " + miniGameCount);
    }

    IEnumerator InvasionComing()
    {
        Debug.Log("Human Invasion coming");

        yield return new WaitForSeconds(10f);

        LoadingImageController.Instance.SetSprite(LoadingImageController.Instance.finalImage);
        LoadingImageController.Instance.SetTips(LoadingImageController.Instance.finalTips);
        loader.LoadSceneByName("FPSMainScene");
    }
}