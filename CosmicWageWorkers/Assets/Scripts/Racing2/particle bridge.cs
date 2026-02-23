using UnityEngine;

public class slimehitEffect : MonoBehaviour
{
    public static slimehitEffect Instance;

    void Awake()
    {
        Instance = this;
    }

    public void PlayEffect()
    {
        GetComponent<ParticleSystem>().Play();
    }
}