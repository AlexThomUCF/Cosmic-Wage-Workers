using UnityEngine;
using UnityEngine.SceneManagement;

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
        //Debug.Log(gravityGameWon);
    }

    // Update is called once per frame
    void Update()
    {

        if (gravityOn)
        {
            sceneLoader -= Time.deltaTime;
            if (sceneLoader <= 0f)
            {
                if (!string.IsNullOrEmpty(interactionID))
                {
                    CustomerManager.MarkInteractionComplete(interactionID);
                }

                //SaveSystem.SaveGame();
               // FinalMiniGame.miniGameCount++;
                
                
                

                Physics.gravity = GravityStore.MainSceneGravity;

                //This will track if player came from gravity scene
                //SceneTracker.Instance.previousScene = SceneManager.GetActiveScene().name;
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
