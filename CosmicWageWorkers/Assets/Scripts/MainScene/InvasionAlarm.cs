using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.Cinemachine;

public class InvasionAlarm : MonoBehaviour
{
    [Header("Cameras")]
    public CinemachineCamera mainCam;
    public CinemachineCamera invasionCam;

    [Header("UI")]
    public GameObject invasionPanel;
    public TextMeshProUGUI dialogueText;
    [TextArea] public string invasionMessage;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip alarmSound;

    public SceneLoader loader;
    public void StartInvasionSequence()
    {
        StartCoroutine(InvasionRoutine());
    }

    public void Update()
    {
        loader = FindFirstObjectByType<SceneLoader>();
    }
    IEnumerator InvasionRoutine()
    {
        mainCam.Priority = 0;
        invasionCam.Priority = 20;

        ForceCameraUpdate();

        yield return new WaitForSeconds(1f);

        if (audioSource != null && alarmSound != null)
            audioSource.PlayOneShot(alarmSound);

        if (invasionPanel != null)
            invasionPanel.SetActive(true);

        if (dialogueText != null)
            dialogueText.text = invasionMessage;

        yield return new WaitForSeconds(3f);

        if (dialogueText != null)
            dialogueText.text = "";

        if (invasionPanel != null)
            invasionPanel.SetActive(false);

        //invasionCam.Priority = 0;
        //mainCam.Priority = 20;

        ForceCameraUpdate();

        yield return new WaitForSeconds(1f);

        LoadingImageController.Instance.SetSprite(LoadingImageController.Instance.finalImage);
        LoadingImageController.Instance.SetTips(LoadingImageController.Instance.finalTips);

        loader.LoadSceneByName("FPSMainScene");
    }

    void ForceCameraUpdate()
    {
        var brain = Camera.main.GetComponent<CinemachineBrain>();
        if (brain != null)
            brain.ManualUpdate();
    }
}