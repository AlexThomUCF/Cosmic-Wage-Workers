using UnityEngine;
using UnityEngine.SceneManagement;

public class FPSManager : MonoBehaviour
{
    public static FPSManager Instance { get; private set; }

    [SerializeField]SceneLoader loader;

    [Header("Win Settings")]
    public int killCount = 0;
    public int killsToWin = 10;
    public string nextSceneName = "MainScene";

    [Header("Customer Interaction ID")]
    public string interactionID; // Assign the ID of the customer associated with this minigame

    private void Awake()
    {
        Instance = this;
        loader = FindFirstObjectByType<SceneLoader>();
    }

    public void RegisterKill()
    {
        killCount++;
        Debug.Log("Kill count: " + killCount);

        if (killCount >= killsToWin)
        {
            // Mark the interaction complete
            if (!string.IsNullOrEmpty(interactionID))
            {
                CustomerManager.MarkInteractionComplete(interactionID);
            }

            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        string titleScreenName = "UI";
        //loader.LoadSceneByName(titleScreenName); doesn't finish fade to main scene for some reason
        SceneManager.LoadScene(titleScreenName);
    }
}