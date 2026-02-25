using UnityEngine;

public class GravityManager : MonoBehaviour
{
    public float gravityScale = -1.0f;
    public GameObject player;
    private bool gravityOn = false;
    public float sceneLoader = 3f;

    public string interactionID; // ID to track the specific interaction for completion



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Physics.gravity = new Vector3(0, gravityScale, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.y < 27.0f)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
            Debug.Log("Restarting Level");
        }

        if (gravityOn)
        {
            sceneLoader -= Time.deltaTime;
            if (sceneLoader <= 0f)
            {
                if (!string.IsNullOrEmpty(interactionID))
                {
                    CustomerManager.MarkInteractionComplete(interactionID);
                }

                FinalMiniGame.miniGameCount++;
                SaveSystem.SaveGame();
                UnityEngine.SceneManagement.SceneManager.LoadScene("MainScene");
                Debug.Log("Loading Next Level");
                sceneLoader = 3f;
                gravityOn = false;
            }
        }
    }
   

    public void GravityTurnedOn()
    {
        gravityOn = true;
        Physics.gravity = new Vector3(0, -9.81f, 0);
    }
}
