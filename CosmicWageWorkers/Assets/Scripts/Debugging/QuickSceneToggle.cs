using UnityEngine;
using UnityEngine.SceneManagement;

public class QuickSceneToggle : MonoBehaviour
{
    private bool inHorrorScene = false;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (inHorrorScene)
            {
                SceneManager.LoadScene("MainScene");
                inHorrorScene = false;
            }
            else
            {
                SceneManager.LoadScene("backroomhorror");
                inHorrorScene = true;
            }
        }
    }
}