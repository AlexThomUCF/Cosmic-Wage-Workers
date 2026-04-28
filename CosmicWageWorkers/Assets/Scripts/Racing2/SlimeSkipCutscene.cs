using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.InputSystem;

public class SlimeSkipCutscene : MonoBehaviour
{
    public PlayableDirector director;
    public MiniGameTimer gameTimer;
    public Canvas uiCanvas;
    public GameObject catchText;
    public TeenAI teenAI;

    private bool hasStartedGame = false;

    void Update()
    {
        // Skip with T
        if (Keyboard.current.tKey.wasPressedThisFrame && director.state == PlayState.Playing)
        {
            director.Stop();
            catchText.SetActive(false);
            if (gameTimer != null)
                gameTimer.StartTimer();
        }

        // When cutscene ends (or is skipped)
        if (!hasStartedGame && director.state != PlayState.Playing)
        {
            hasStartedGame = true;
            teenAI.alwaysFlee = true;

            if (uiCanvas != null)
                uiCanvas.gameObject.SetActive(true);

            if (gameTimer != null)
                gameTimer.StartTimer();
        }
    }
}