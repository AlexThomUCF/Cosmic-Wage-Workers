using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AlarmUIManager : MonoBehaviour
{
    public Image[] alarmSlots;

    [Header("Colors")]
    public Color idleColor = Color.gray;
    public Color activeColor = Color.red;

    [Header("Flash Settings")]
    public float flashOnTime = 0.15f;
    public float flashOffTime = 0.15f;

    private Coroutine flashRoutine;
    private int activeSlotCount = 0;

    void Start()
    {
        ResetUI();
    }

    public void ResetUI()
    {
        StopFlashing();

        foreach (var img in alarmSlots)
            img.color = idleColor;

        activeSlotCount = 0;
    }

    //  Reveal phase  TURN ON and KEEP ON
    public void SetSlotActive(int index)
    {
        if (index >= alarmSlots.Length) return;

        alarmSlots[index].color = activeColor;

        // track how many slots are in play
        if (index + 1 > activeSlotCount)
            activeSlotCount = index + 1;
    }

    //  Active phase  ONLY flash used slots
    public void StartActiveFlashing()
    {
        StopFlashing();
        flashRoutine = StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        while (true)
        {
            // ON
            for (int i = 0; i < activeSlotCount; i++)
                alarmSlots[i].color = activeColor;

            yield return new WaitForSeconds(flashOnTime);

            // OFF
            for (int i = 0; i < activeSlotCount; i++)
                alarmSlots[i].color = idleColor;

            yield return new WaitForSeconds(flashOffTime);
        }
    }

    public void StopFlashing()
    {
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);
    }

    //  Player hits correct one
    public void MarkSuccess(int index)
    {
        if (index < alarmSlots.Length)
            alarmSlots[index].color = Color.green;
    }
}