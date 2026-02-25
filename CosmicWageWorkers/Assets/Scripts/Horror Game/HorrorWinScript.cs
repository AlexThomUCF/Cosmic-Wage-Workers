using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class HorrorWinScript : MonoBehaviour
{
    [Header("Customer Interaction ID")]
    public string interactionID; // Assign the ID of the customer for this minigame
    [SerializeField] SceneLoader loader;
    [SerializeField] private UnityEvent eventObj;
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
            FinalMiniGame.miniGameCount++;
            SaveSystem.SaveGame();
            // Load the main scene

            eventObj.Invoke();

            string mainSceneName = "MainScene";
            loader.LoadSceneByName(mainSceneName);
           // SceneManager.LoadScene(mainSceneName);
        }
    }
}