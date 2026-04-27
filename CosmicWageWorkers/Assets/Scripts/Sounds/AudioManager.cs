using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer myMixer;
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Slider voiceSlider;

    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;
    [SerializeField] AudioSource voiceSource;

    public AudioClip background;
    public AudioClip buttonPress;
    public AudioClip helloThere;

    void Start()
    {
        musicSource.clip = background;
        musicSource.Play();

        if (PlayerPrefs.HasKey("masterVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetDefaultVolumes();
        }
    }

    void Update()
    {
        if (SFXSource == null)
        {
            SFXSource = FindFirstObjectByType<SoundEffectManager>().GetComponent<AudioSource>();
        }
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }

    public void PlayVoice(AudioClip clip)
    {
        voiceSource.PlayOneShot(clip);
    }

    public void SetMasterVolume()
    {
        float volume = Mathf.Clamp(masterSlider.value, 0.0001f, 1f);
        myMixer.SetFloat("masterVol", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("masterVolume", volume);
    }

    public void SetMusicVolume()
    {
        float volume = Mathf.Clamp(musicSlider.value, 0.0001f, 1f);
        myMixer.SetFloat("musicVol", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume()
    {
        float volume = Mathf.Clamp(sfxSlider.value, 0.0001f, 1f);
        myMixer.SetFloat("SFXVol", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void SetVoiceVolume()
    {
        float volume = Mathf.Clamp(voiceSlider.value, 0.0001f, 1f);
        myMixer.SetFloat("voiceVol", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("VoiceVolume", volume);
    }

    private void LoadVolume()
    {
        masterSlider.value = PlayerPrefs.GetFloat("masterVolume");
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");
        voiceSlider.value = PlayerPrefs.GetFloat("VoiceVolume");

        SetMasterVolume();
        SetMusicVolume();
        SetSFXVolume();
        SetVoiceVolume();
    }

    private void SetDefaultVolumes()
    {
        masterSlider.value = 0.5f;
        musicSlider.value = 0.5f;
        sfxSlider.value = 0.5f;
        voiceSlider.value = 0.5f;

        SetMasterVolume();
        SetMusicVolume();
        SetSFXVolume();
        SetVoiceVolume();
    }
}