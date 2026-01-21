using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class GlobalVolumeController : MonoBehaviour
{
    public AudioMixer masterMixer;
    [SerializeField] private Slider sfxsSlider;

    // volume between 0 (full volume) and 1 (silent)
    public void SetMasterVolume(float volume)
    {
        // Convert slider (0..1) to mixer (0 to -80 dB)
        float dB = Mathf.Lerp(-80f, 0f, volume);
        masterMixer.SetFloat("Volume", dB);
    }
}
