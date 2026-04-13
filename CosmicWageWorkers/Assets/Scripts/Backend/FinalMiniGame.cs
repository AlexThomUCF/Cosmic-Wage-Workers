using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinalMiniGame : MonoBehaviour
{
    [SerializeField] private int numberOfMinigames = 0;

    private static FinalMiniGame Instance;
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
    }

    void Update()
    {
        if (!invasionStarted && miniGameCount == numberOfMinigames)
        {
            invasionStarted = true;
            StartCoroutine(InvasionComing());
        }
        Debug.Log("Minigame count = " + miniGameCount);
    }

    IEnumerator InvasionComing()
    {
        Debug.Log("Human Invasion coming");

        yield return new WaitForSeconds(10f);

        SceneManager.LoadScene("FPSMainScene");
    }
}