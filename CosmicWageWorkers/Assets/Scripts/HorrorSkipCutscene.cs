using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;
using UnityEngine.Playables;


public class HorrorSkipCutscene : MonoBehaviour
{
    [Header("Global References")]
    public PlayableDirector director;
    public Interaction interaction;

    [Header("Horror References")]
    public FlashLight fLight;
    public GameObject horrorMonster;


    public void Start()
    {
        interaction.enabled = false;
        

        if (fLight != null)
        {
            fLight.enabled = false;
        }
        if(horrorMonster != null)
        {
            horrorMonster.SetActive(false);
        }    
    }
    void Update()
    {
        if (Keyboard.current.tKey.wasPressedThisFrame && director.state == PlayState.Playing)
        {
            director.Stop();
            interaction.enabled = true;
            if (fLight != null)
            {
                fLight.enabled = true;
            }
            if(horrorMonster != null)
            {
                horrorMonster.SetActive(true);
            }

        }
        else if (director.state != PlayState.Playing)
        {
            interaction.enabled = true;
            if (fLight != null)
            {
                fLight.enabled = true;
            }
            if (horrorMonster != null)
            {
                horrorMonster.SetActive(true);
            }

        }


    }

}
