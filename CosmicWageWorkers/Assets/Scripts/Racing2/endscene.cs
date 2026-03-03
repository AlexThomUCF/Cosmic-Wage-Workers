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

    public void PlayAnim()
    {
        teen.SetActive(true);
        origin_teen.SetActive(false);
        c1.gameObject.SetActive(false);
        c2.gameObject.SetActive(true);
        Invoke("ShowWinUI", 3f);
    }

    public void ShowWinUI()
    {
        winui.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void BackToMain()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }
}