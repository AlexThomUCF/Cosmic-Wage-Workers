using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer))]
[RequireComponent(typeof(AudioSource))]
public class AlarmNode : MonoBehaviour
{
    public int alarmID;
    public AlarmSequenceManager manager;

    [Header("Materials")]
    public Material idleMat;
    public Material activeMat;

    [Header("Flicker Settings")]
    public float flickerSpeed = 0.2f;

    [Header("Audio (Reveal Only)")]
    public AudioClip alarmClip;
    public float volume = 0.8f;
    [Tooltip("Length of a single beep")]
    public float beepLength = 0.15f;
    [Tooltip("Gap between each beep")]
    public float beepGap = 0.1f;
    [Tooltip("How many beeps per alarm")]
    public int beepCount = 3;

    private MeshRenderer rend;
    private AudioSource audioSource;
    private Coroutine flickerRoutine;
    private Coroutine audioRoutine;
    private bool isActive = false;

    void Awake()
    {
        rend = GetComponent<MeshRenderer>();
        audioSource = GetComponent<AudioSource>();

        audioSource.playOnAwake = false;
        audioSource.loop = false;
        audioSource.spatialBlend = 0f; // 2D
        audioSource.volume = volume;

        SetIdle();
    }

    // ------------------------
    // Visual States
    // ------------------------

    public void SetIdle()
    {
        isActive = false;

        if (flickerRoutine != null)
            StopCoroutine(flickerRoutine);

        StopAudioImmediate();
        rend.material = idleMat;
    }

    // MEMORY PHASE — triple beep
    public void Reveal()
    {
        if (flickerRoutine != null)
            StopCoroutine(flickerRoutine);

        rend.material = activeMat;
        PlayTripleBeep();
    }

    // EXECUTION PHASE — flicker, no audio
    public void SetActive()
    {
        isActive = true;

        // Stop any audio that might still be playing
        StopAudioImmediate();

        if (flickerRoutine != null)
            StopCoroutine(flickerRoutine);

        flickerRoutine = StartCoroutine(Flicker());
    }

    // ------------------------
    // Flicker
    // ------------------------
    IEnumerator Flicker()
    {
        bool red = true;

        while (isActive)
        {
            rend.material = red ? activeMat : idleMat;
            red = !red;
            yield return new WaitForSeconds(flickerSpeed);
        }
    }

    // ------------------------
    // Audio
    // ------------------------
    void PlayTripleBeep()
    {
        if (alarmClip == null) return;

        StopAudioImmediate();

        audioRoutine = StartCoroutine(TripleBeepCoroutine());
    }

    IEnumerator TripleBeepCoroutine()
    {
        if (alarmClip == null) yield break;

        for (int i = 0; i < beepCount; i++)
        {
            // Forcefully reset the audio
            audioSource.Stop();
            audioSource.clip = alarmClip;
            audioSource.Play();

            // Wait only the length of the beep
            yield return new WaitForSeconds(beepLength);

            // Stop immediately so it doesn’t linger
            audioSource.Stop();

            // Wait gap before next beep
            yield return new WaitForSeconds(beepGap);
        }
    }



    void StopAudioImmediate()
    {
        if (audioRoutine != null)
            StopCoroutine(audioRoutine);

        audioSource.Stop();
    }

    // ------------------------
    // Hit logic
    // ------------------------
    public void OnShot()
    {
        if (!isActive)
            return;

        manager.RegisterHit(alarmID);
    }
   
    public void RevealCutscene()
    {
        // Only play visual and audio, don't affect sequence manager
        if (flickerRoutine != null)
            StopCoroutine(flickerRoutine);

        rend.material = activeMat;
        if (alarmClip != null)
            audioSource.PlayOneShot(alarmClip, volume);
    }
}











