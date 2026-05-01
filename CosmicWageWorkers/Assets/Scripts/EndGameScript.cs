using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class EndGameScript : MonoBehaviour
{
    public GameObject goodEnding;
    public GameObject badEnding;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        goodEnding.SetActive(false);
        badEnding.SetActive(false);
    }

    public void GoodEnding()
    {
        SceneManager.LoadScene("UI");
    }

    public void BadEnding()
    {
        StartCoroutine(ActiveObject(badEnding));
    }

    IEnumerator ActiveObject(GameObject cine)
    {
        yield return new WaitForSeconds(2f);

        cine.gameObject.SetActive(true);

        yield return new WaitForSeconds(5f);

        SceneManager.LoadScene("UI");
    }
}
