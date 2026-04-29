using UnityEngine;

public class CutsceneEnder : MonoBehaviour
{
    public GameObject beginningCutscene;
    public float startingTime;
    public float cutsceneDuration;
    public bool cutsceneStarted = true;
    public GameObject spawner;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (cutsceneStarted)
        { 
            cutsceneDuration -= Time.deltaTime;
            if (cutsceneDuration <= 0)
            {
                cutsceneStarted = false;
                beginningCutscene.SetActive(false);
                cutsceneDuration = startingTime;
                spawner.SetActive(true);    
            }
        }
    }
}
