using UnityEngine;

public class PauseAnimationRelay : MonoBehaviour
{
    public PauseMenu pauseMenu; // Drag PauseManager here in inspector

    public void OnPauseAnimationFinished()
    {
        pauseMenu.OnPauseAnimationFinished();
    }

    public void OnUnpauseAnimationFinished()
    {
        pauseMenu.OnUnpauseAnimationFinished();
    }
}
