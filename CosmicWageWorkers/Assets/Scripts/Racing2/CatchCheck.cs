using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CatchCheck : MonoBehaviour
{
    public string interactionID;
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;
        StartCoroutine(Switch());
    }

    private IEnumerator Switch()
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1f;

        if (!string.IsNullOrEmpty(interactionID))
        {
            CustomerManager.MarkInteractionComplete(interactionID);
        }

        FinalMiniGame.miniGameCount++;

        SceneManager.LoadScene("MainScene"); // direct load, no loader
    }
}