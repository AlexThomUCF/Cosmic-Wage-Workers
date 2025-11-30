using UnityEngine;
using UnityEngine.SceneManagement;

public class QuickSceneToggle : MonoBehaviour
{
    private bool inBackroom = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (!inBackroom)
            {
                SceneManager.LoadScene("backroomhorror");
                inBackroom = true;
            }
            else
            {
                SceneManager.LoadScene("MainScene");
                inBackroom = false;
            }
        }
    }
}
