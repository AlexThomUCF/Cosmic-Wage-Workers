using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundEffectManager : MonoBehaviour
{
    private static SoundEffectManager Instance;
    private static AudioSource audioSource;
    private static AudioSource voiceAudioSource;
    private static SoundEffectLibrary soundEffectLibrary;
    [SerializeField] private  Slider sfxsSlider;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            AudioSource[] audioSources = GetComponents<AudioSource>();
            audioSource = audioSources[0];
            voiceAudioSource = audioSources[1];
            soundEffectLibrary = GetComponent<SoundEffectLibrary>();

            DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);

        }
    }

    private void Start()
    {
        sfxsSlider.onValueChanged.AddListener(delegate { OnValueChange(); });
    }

    public static void Play(string soundName)
    {
        AudioClip audioClip = soundEffectLibrary.GetRandomClip(soundName);
        if (audioClip != null)
        {
            audioSource.PlayOneShot(audioClip);
        }
    }

    public static void PlayVoice(AudioClip audioClip, float pitch = 1f)
    {
        voiceAudioSource.pitch = pitch;
        voiceAudioSource.PlayOneShot(audioClip);
    }

    public static void SetVolume(float volume)
    {
        audioSource.volume = volume;
        voiceAudioSource.volume = volume;
    }

    public  void OnValueChange()
    {
        SetVolume(sfxsSlider.value);
    }
}
