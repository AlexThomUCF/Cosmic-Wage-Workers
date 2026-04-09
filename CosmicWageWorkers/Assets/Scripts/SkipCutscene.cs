using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;

public class SkipCutscene : MonoBehaviour
{
    public PlayableDirector director;

    void Update()
    {
        if (Keyboard.current.tKey.wasPressedThisFrame && director.state == PlayState.Playing)
        {
            director.Stop();
        }
    }
}