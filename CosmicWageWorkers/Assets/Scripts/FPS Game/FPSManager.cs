using UnityEngine;
using UnityEngine.SceneManagement;

public class FPSManager : MonoBehaviour
{
    public static FPSManager Instance { get; private set; }

    [Header("Win Settings")]
    public int killCount = 0;
    public int killsToWin = 10;
    public string nextSceneName = "ProgramPrototype";

    private void Awake()
    {
        // Basic singleton that resets each scene
        Instance = this;
    }

    public void RegisterKill()
    {
        killCount++;
        Debug.Log("Kill count: " + killCount);

        if (killCount >= killsToWin)
        {
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}