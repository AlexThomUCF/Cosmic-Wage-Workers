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
    public Material successMat;
    public float successFlashTime = 0.2f;

    public float repeatDelay = 1.5f; // delay between repeats when active

    [Header("Flicker Settings")]
    public float flickerSpeed = 0.2f;

    [Header("Audio (Reveal Only)")]
    public AudioClip alarmClip;
    public float volume = 0.8f;

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

        audioSource.Stop();

        rend.material = idleMat;
    }

        public void Reveal()
    {
        if (flickerRoutine != null)
            StopCoroutine(flickerRoutine);

        rend.material = activeMat;

        PlaySoundOnce();
    }

        public void SetActive()
    {
        isActive = true;

        if (flickerRoutine != null)
            StopCoroutine(flickerRoutine);

        flickerRoutine = StartCoroutine(Flicker());

        StartCoroutine(LoopSound());
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

    void StopAudioImmediate()
    {
        if (audioRoutine != null)
            StopCoroutine(audioRoutine);

        audioSource.Stop();
    }

        public void OnShot()
    {
        if (!isActive) return;

        isActive = false;

        if (flickerRoutine != null)
            StopCoroutine(flickerRoutine);

        audioSource.Stop();

        StartCoroutine(SuccessFlash());

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

        void PlaySoundOnce()
    {
        if (alarmClip == null) return;

        audioSource.Stop();
        audioSource.clip = alarmClip;
        audioSource.loop = false;
        audioSource.Play();
    }

    IEnumerator LoopSound()
    {
        while (isActive)
        {
            PlaySoundOnce();
            yield return new WaitForSeconds(alarmClip.length + repeatDelay);
        }
    }

        IEnumerator SuccessFlash()
    {
        rend.material = successMat;

        yield return new WaitForSeconds(successFlashTime);

        rend.material = idleMat;
    }
}