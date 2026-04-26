using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class endscene : MonoBehaviour
{
    public GameObject teen;
    public GameObject origin_teen;
    public Camera c1;
    public Camera c2;
    public GameObject winui;
    public string sceneName;
    public SceneLoader loader;


    public void Start()
    {
        //loader = FindAnyObjectByType<SceneLoader>();
    }
    public void Update()
    {
        loader = FindAnyObjectByType<SceneLoader>();
    }

    public void PlayAnim()
    {
        
        /*teen.SetActive(true);
        origin_teen.SetActive(false);
        c1.gameObject.SetActive(false);
        c2.gameObject.SetActive(true);
        Invoke("ShowWinUI", 3f);*/
        StartCoroutine(LoadBack());
    }

    public void ShowWinUI()
    {
        //winui.SetActive(true);

        //Cursor.lockState = CursorLockMode.None;
        //Cursor.visible = true;
    }

    public IEnumerator LoadBack()
    {
        teen.SetActive(true);
        origin_teen.SetActive(false);
        c1.gameObject.SetActive(false);
        c2.gameObject.SetActive(true);
        Invoke("ShowWinUI", 3f);
        yield return new WaitForSeconds(4f);
        loader.LoadSceneByName(sceneName);
        
    }

    public void BackToMain()
    {
        Time.timeScale = 1f;
        //SceneManager.LoadScene(sceneName);
    }
}