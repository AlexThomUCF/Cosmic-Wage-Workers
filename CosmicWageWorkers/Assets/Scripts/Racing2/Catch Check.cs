using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FreezeAndSwitchScene : MonoBehaviour
{
    [SerializeField] SceneLoader loader;

    public string interactionID; // ID to track the specific interaction for completion
    private bool triggered = false;

    public void Awake()
    {
        loader = FindAnyObjectByType<SceneLoader>();
    }

    private void OnTriggerEnter(Collider other) // detect player hit
    {
        if (triggered) return;        // prevent multiple triggers
        if (!other.CompareTag("Player")) return;

        triggered = true;             // mark as triggered
        StartCoroutine(Switch());
    }

    private IEnumerator Switch()
    {
        //Time.timeScale = 0f; // freeze game
        yield return new WaitForSecondsRealtime(1f);
        //Time.timeScale = 1f; // restore

        if (!string.IsNullOrEmpty(interactionID))
        {
            CustomerManager.MarkInteractionComplete(interactionID);
        }

        FinalMiniGame.miniGameCount++;
        string mainSceneName = "MainScene";
        loader.LoadSceneByName(mainSceneName);
        
    }
}
