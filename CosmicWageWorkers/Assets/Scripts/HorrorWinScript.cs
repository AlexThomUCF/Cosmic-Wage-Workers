using UnityEngine;
using UnityEngine.SceneManagement;

public class HorrorWinScript : MonoBehaviour
{
    [Header("Customer Interaction ID")]
    public string interactionID; // Assign the ID of the customer for this minigame

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
            SceneManager.LoadScene(mainSceneName);
        }
    }
}