using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FreezeAndSwitchScene : MonoBehaviour
{
    [SerializeField] SceneLoader loader;

    public void Awake()
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
        //Time.timeScale = 0f; // freeze game
        yield return new WaitForSecondsRealtime(1f);
        //Time.timeScale = 1f; // restore
        FinalMiniGame.miniGameCount++;
        string mainSceneName = "MainScene";
        loader.LoadSceneByName(mainSceneName);
        
    }
}
