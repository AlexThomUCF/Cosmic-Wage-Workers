using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FreezeAndSwitchScene : MonoBehaviour
{
    [SerializeField] SceneLoader loader;
    public string interactionID; // Assign the ID of the customer for this minigame

    public void Awake()
    {
        loader = FindAnyObjectByType<SceneLoader>();
    }

    public void Start()
    {
        loader = FindAnyObjectByType<SceneLoader>();
    }

    private void OnTriggerEnter(Collider other) //detect player hit
    {
        if (!other.CompareTag("Player")) return;
        StartCoroutine(Switch());
    }

    private IEnumerator Switch()
    {
        Time.timeScale = 0f; // freeze game
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1f; // restore
        
        // Mark the interaction complete
        if (!string.IsNullOrEmpty(interactionID))
        {
            CustomerManager.MarkInteractionComplete(interactionID);
        }

        FinalMiniGame.miniGameCount++;
        string mainSceneName = "MainScene";
        loader.LoadSceneByName(mainSceneName);
        
    }
}
