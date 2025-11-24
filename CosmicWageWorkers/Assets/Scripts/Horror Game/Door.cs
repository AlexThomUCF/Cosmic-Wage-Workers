using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    [Header("Customer Interaction ID")]
    public string interactionID; // Assign the ID of the customer for this minigame

    public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") && RealItem.hasItem == true)
        {
            // Mark the interaction complete
            if (!string.IsNullOrEmpty(interactionID))
            {
                CustomerManager.MarkInteractionComplete(interactionID);
            }

            // Load the next scene
            SceneManager.LoadScene("POCScene");
        }
    }
}
