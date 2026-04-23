using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;

public class SkipCutscene : MonoBehaviour
{
    public PlayableDirector director;
    public GameObject border;

    void Update()
    {
        if (Keyboard.current.tKey.wasPressedThisFrame && director.state == PlayState.Playing)
        {
            director.Stop();
            border.SetActive(false);
        }

        if(director.state != PlayState.Playing)
        {
            border.SetActive(false);
        }
    }
}