using UnityEngine;

public class Bell : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float cooldown = 1f;

    private float lastRingTime = -999f;

    public void Ring()
    {
        if (Time.time - lastRingTime < cooldown)
            return;

        lastRingTime = Time.time;

        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}
