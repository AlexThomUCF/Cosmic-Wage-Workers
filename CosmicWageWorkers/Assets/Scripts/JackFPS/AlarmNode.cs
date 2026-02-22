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
        audioSource.spatialBlend = 0f;
        audioSource.volume = volume;

        SetIdle();
    }

    public void SetIdle()
    {
        isActive = false;

        if (flickerRoutine != null)
            StopCoroutine(flickerRoutine);

        StopAudioImmediate();
        rend.material = idleMat;
    }

    public void Reveal()
    {
        if (flickerRoutine != null)
            StopCoroutine(flickerRoutine);

        rend.material = activeMat;
        PlayTripleBeep();
    }

    public void SetActive()
    {
        isActive = true;

        StopAudioImmediate();

        if (flickerRoutine != null)
            StopCoroutine(flickerRoutine);

        flickerRoutine = StartCoroutine(Flicker());
    }

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

    void PlayTripleBeep()
    {
        if (alarmClip == null) return;

        StopAudioImmediate();
        audioRoutine = StartCoroutine(TripleBeepCoroutine());
    }

    IEnumerator TripleBeepCoroutine()
    {
        for (int i = 0; i < beepCount; i++)
        {
            audioSource.Stop();
            audioSource.clip = alarmClip;
            audioSource.Play();

            yield return new WaitForSeconds(beepLength);

            audioSource.Stop();
            yield return new WaitForSeconds(beepGap);
        }
    }

    void StopAudioImmediate()
    {
        if (audioRoutine != null)
            StopCoroutine(audioRoutine);

        audioSource.Stop();
    }

    public void OnShot()
    {
        if (!isActive) return;
        manager.RegisterHit(alarmID);
    }

    public void RevealCutscene()
    {
        if (flickerRoutine != null)
            StopCoroutine(flickerRoutine);

        isActive = false;

        if (rend != null && activeMat != null)
            rend.material = activeMat;

        if (alarmClip != null && audioSource != null)
        {
            audioSource.Stop();
            audioSource.PlayOneShot(alarmClip, volume);
        }

        StartCoroutine(CutsceneFlash());
    }

    IEnumerator CutsceneFlash()
    {
        yield return new WaitForSeconds(0.25f);
        if (rend != null && idleMat != null)
            rend.material = idleMat;
    }
}