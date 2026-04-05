using UnityEngine;
using UnityEngine.Playables;

public class SkipCutscene : MonoBehaviour
{
    public PlayableDirector director;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T) && director.state == PlayState.Playing)
        {
            director.Stop();
        }
    }
}