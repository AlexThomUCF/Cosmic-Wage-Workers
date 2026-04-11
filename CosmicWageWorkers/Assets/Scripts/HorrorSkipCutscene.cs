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


    public void Start()
    {
        interaction.enabled = false;

        if (fLight != null)
        {
            fLight.enabled = false;
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

        }
        else if (director.state != PlayState.Playing)
        {
            interaction.enabled = true;
            if (fLight != null)
            {
                fLight.enabled = true;
            }

        }


    }

}
