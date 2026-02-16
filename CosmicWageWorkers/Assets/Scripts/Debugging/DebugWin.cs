using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugWin : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
        {
            DebugLoad();
        }
    }

    private void DebugLoad()
    {
        // Increment mini game counter
        FinalMiniGame.miniGameCount++;

        // Load MainScene
        SceneManager.LoadScene("MainScene");
    }
}