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
<<<<<<< HEAD
        
        StartCoroutine(Switch(other.gameObject));

=======
        catchsound.Play();
        StartCoroutine(Switch(other.gameObject));
>>>>>>> 5294a35 (add climatic camera)
    }

    private IEnumerator Switch(GameObject player)
    {
<<<<<<< HEAD
        catchsound.Play();
        yield return new WaitForSecondsRealtime(2f);
        FindObjectOfType<endscene>().PlayAnim();
        yield return new WaitForSecondsRealtime(5f);

=======
>>>>>>> 5294a35 (add climatic camera)
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(1f);
        Time.timeScale = 1f;

        if (!string.IsNullOrEmpty(interactionID))
        {
            CustomerManager.MarkInteractionComplete(interactionID);
        }

        FinalMiniGame.miniGameCount++;
        SaveSystem.SaveGame();

        if (climaticCamera != null)
            climaticCamera.SetActive(true);

        player.SetActive(false);
    }
}