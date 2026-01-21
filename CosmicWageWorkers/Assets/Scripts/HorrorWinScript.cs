using UnityEngine;
using UnityEngine.SceneManagement;

public class HorrorWinScript : MonoBehaviour
{
    [Header("Customer Interaction ID")]
    public string interactionID; // Assign the ID of the customer for this minigame
    [SerializeField] SceneLoader loader;
    public void Awake()
    {
        loader = FindAnyObjectByType<SceneLoader>();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && RealItem.hasItem)
        {
            Debug.Log("Player has won");

            // Mark the interaction complete
            if (!string.IsNullOrEmpty(interactionID))
            {
                CustomerManager.MarkInteractionComplete(interactionID);
            }

            // Load the main scene
            Debug.Log("Before increment: " + FinalMiniGame.miniGameCount);
            FinalMiniGame.miniGameCount++;
            Debug.Log("After increment: " + FinalMiniGame.miniGameCount);

            string mainSceneName = "MainScene";
            loader.LoadSceneByName(mainSceneName);
            Debug.Log("After Scene Load: " + FinalMiniGame.miniGameCount);
            //SceneManager.LoadScene(mainSceneName);
        }
    }
}