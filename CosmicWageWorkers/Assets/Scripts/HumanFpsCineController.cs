 using UnityEngine;
using UnityEngine.Playables;
public class HumanFpsCineController : MonoBehaviour
{
    public WaveSpawner waveSpawner;
    public PlayableDirector startCine;
    public PlayableDirector endCine;
    public GameObject dialogue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Awake()
    {
        waveSpawner.enabled = false;
        endCine.gameObject.SetActive(false);
        dialogue.SetActive(false);
    }
    void Start()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
       if (startCine.state != PlayState.Playing)
        {
            startCine.gameObject.SetActive(false);
            waveSpawner.enabled = true;
        }
    }


}
