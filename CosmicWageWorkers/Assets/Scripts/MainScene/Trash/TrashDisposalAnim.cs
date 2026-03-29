using UnityEngine;

public class TrashDispose : MonoBehaviour
{
    [SerializeField] private Animator targetAnimator;
    [SerializeField] private string[] animationStateNames;

    public void PlayAnimationFromList()
    {
        string stateName = animationStateNames[Random.Range(0, animationStateNames.Length)];
        targetAnimator.Play(stateName, 0, 0f);
    }
}
