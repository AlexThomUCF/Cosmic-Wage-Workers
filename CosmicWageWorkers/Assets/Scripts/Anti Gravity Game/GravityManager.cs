using UnityEngine;

public class GravityManager : MonoBehaviour
{
    public float gravityScale = -1.0f; // Mini-game gravity
    public GameObject player;
    private bool gravityOn = false;
    public float sceneLoader = 3f;

    public string interactionID; // ID to track the specific interaction for completion

    private Vector3 originalGravity; // Store the original gravity

    void Start()
    {
        Physics.gravity = new Vector3(0, gravityScale, 0);

    }

    void Update()
    {
        if (gravityOn)
        {
            sceneLoader -= Time.deltaTime;
            if (sceneLoader <= 0f)
            {
                // Mark interaction complete
                if (!string.IsNullOrEmpty(interactionID))
                {
                    CustomerManager.MarkInteractionComplete(interactionID);
                }

                FinalMiniGame.miniGameCount++;
                SaveSystem.SaveGame();
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
                Debug.Log("Loading Next Level");

                // Reset timer and gravity state
                sceneLoader = 3f;
                gravityOn = false;
            }
        }
    }

    public void GravityTurnedOn()
    {
        gravityOn = true;
        Physics.gravity = new Vector3(0, -9.81f, 0); // Mini-game gravity
    }
}