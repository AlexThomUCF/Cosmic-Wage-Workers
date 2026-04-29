using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SaveSlotUI : MonoBehaviour
{
    public TextMeshProUGUI fileText;
    public string sceneToLoad = "MainScene";

    void Start()
    {
        if (SaveSystem.SaveExists())
        {
            fileText.text = "Continue";
        }
        else
        {
            fileText.text = "Empty";
        }
    }

    public void OnFile1Pressed()
    {
        if (SaveSystem.SaveExists())
        {
            SaveSystem.LoadGame();
        }
        else
        {
            // Optional: clear old data just in case
            FinalMiniGame.miniGameCount = 0;
            CustomerManager.SetCompletedInteractions(new System.Collections.Generic.List<string>());
        }

        SceneManager.LoadScene(sceneToLoad);
    }
}