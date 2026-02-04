using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FreezeAndSwitchScene : MonoBehaviour
{


    private void OnTriggerEnter(Collider other) //detect player hit
    {
        if (!other.CompareTag("Player")) return;
        StartCoroutine(Switch());
    }

    private IEnumerator Switch()
    {
        Time.timeScale = 0f; // freeze game
        yield return new WaitForSecondsRealtime(3);
        Time.timeScale = 1f; // restore
        SceneManager.LoadScene("MainScene");
    }
}
