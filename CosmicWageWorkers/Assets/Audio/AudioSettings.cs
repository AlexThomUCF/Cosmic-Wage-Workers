using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Audio;

public class AudioSettingsUI : MonoBehaviour
{
    [SerializeField] UIDocument titleDocument;
    private VisualElement root;
    public AudioMixer audioMixer;

    private void OnEnable()
    {
        root = titleDocument.rootVisualElement;

        var masterSlider = root.Q<Slider>("MainVolume");

        var musicSlider = root.Q<Slider>("Music");

        var sfxSlider = root.Q<Slider>("SFX");

        var voiceSlider = root.Q<Slider>("Voices");

   
        masterSlider.RegisterValueChangedCallback(evt =>
        {
            audioMixer.SetFloat("masterVol", LinearToDecibel(evt.newValue));
        });

        musicSlider.RegisterValueChangedCallback(evt =>
        {
            audioMixer.SetFloat("musicVol", LinearToDecibel(evt.newValue));
        });

        sfxSlider.RegisterValueChangedCallback(evt =>
        {
            audioMixer.SetFloat("SFXVol", LinearToDecibel(evt.newValue));
        });

        voiceSlider.RegisterValueChangedCallback(evt =>
        {
            audioMixer.SetFloat("voiceVol", LinearToDecibel(evt.newValue));
        });
    }
    private float LinearToDecibel(float linear)
    {
        return Mathf.Log10(Mathf.Clamp(linear, 0.0001f, 1f)) * 20f;
    }
}