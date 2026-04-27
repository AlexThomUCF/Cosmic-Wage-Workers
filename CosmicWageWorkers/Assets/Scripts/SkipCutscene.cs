using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;

public class SkipCutscene : MonoBehaviour
{
    public PlayableDirector director;
    public GameObject border;
    public GameObject disableObject;

    void Update()
    {
        if (Keyboard.current.tKey.wasPressedThisFrame && director.state == PlayState.Playing)
        {
            director.Stop();
            if(disableObject != null)
            {
                disableObject.SetActive(false);
            }
            if(border != null)
            {
                border.SetActive(false);
            }    
            
        }

        if(director.state != PlayState.Playing)
        {
            border.SetActive(false);
        }
    }
}