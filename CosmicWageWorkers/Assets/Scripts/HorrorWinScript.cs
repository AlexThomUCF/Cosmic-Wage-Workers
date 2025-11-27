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
            
            string mainSceneName = "POCScene";
            loader.LoadSceneByName(mainSceneName);
            //SceneManager.LoadScene(mainSceneName);
        }
    }
}