using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;

public class SlimeSkipCutscene : MonoBehaviour
{
    public PlayableDirector director;
    public MiniGameTimer gameTimer;
    public Canvas uiCanvas;

    private bool hasStartedGame = false;

    void Update()
    {
        // Skip with T
        if (Keyboard.current.tKey.wasPressedThisFrame && director.state == PlayState.Playing)
        {
            director.Stop();
        }

        // When cutscene ends (or is skipped)
        if (!hasStartedGame && director.state != PlayState.Playing)
        {
            hasStartedGame = true;

            if (uiCanvas != null)
                uiCanvas.gameObject.SetActive(true);

            if (gameTimer != null)
                gameTimer.StartTimer();
        }
    }
}