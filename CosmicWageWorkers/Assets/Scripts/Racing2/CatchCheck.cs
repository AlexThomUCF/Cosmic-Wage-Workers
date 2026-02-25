using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class CatchCheck : MonoBehaviour
{
    public string interactionID;
    public AudioSource catchsound;
    public GameObject climaticCamera;

    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("Player")) return;

        triggered = true;
        catchsound.Play();
        StartCoroutine(Switch(other.gameObject));
    }

    private IEnumerator Switch(GameObject player)
    {
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1f;

        if (!string.IsNullOrEmpty(interactionID))
        {
            CustomerManager.MarkInteractionComplete(interactionID);
        }

        FinalMiniGame.miniGameCount++;

        if (climaticCamera != null)
            climaticCamera.SetActive(true);

        player.SetActive(false);
    }
}