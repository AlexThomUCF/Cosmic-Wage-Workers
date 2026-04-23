using UnityEngine;
using UnityEngine.Playables;

public class SceneMainController : MonoBehaviour
{   
    public PlayableDirector director;
    public GameObject cam;
    void Start()
    {
        if (SceneTracker.Instance != null &&
            SceneTracker.Instance.previousScene == "AGFinal" && PortalTrigger.gravityGameWon)
        {
            cam.SetActive(true);
            Debug.Log("Came from Scene B");
            director.Play();
           
            // Do special logic here
        }


    }

    private void Update()
    {
        if (director.state != PlayState.Playing)
        {
            cam.SetActive(false);
        }
    }
}
