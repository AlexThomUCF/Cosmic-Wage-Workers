using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmicPhenomenonManager : MonoBehaviour
{
    [Header("Timing")]
    public float minTimeBetweenEvents = 30f;
    public float maxTimeBetweenEvents = 60f;

    [Header("References")]
    public SolarFlare solarFlare;
    public AntiGravity antiGravity;
    public BlackHoles blackHoles;
    public Eclipse eclipse;
    public PrimordialSoup primordialSoup;

    private void Start()
    {
        StartCoroutine(EventLoop());
    }

    private IEnumerator EventLoop()
    {
        while (true)
        {
            float waitTime = Random.Range(minTimeBetweenEvents, maxTimeBetweenEvents);
            yield return new WaitForSeconds(waitTime);

            TriggerRandomEvent();
        }
    }

    private void TriggerRandomEvent()
    {
        int eventIndex = Random.Range(0, 5); // 0 = SolarFlare, 1 = AntiGravity, 2 = BlackHoles, 3 = Eclipse, 4 = PrimordialSoup

        switch (eventIndex)
        {
            case 0:
                if (solarFlare != null) solarFlare.TriggerFlare();
                break;
            case 1:
                if (antiGravity != null) antiGravity.TriggerAntiGravity();
                break;
            case 2:
                if (blackHoles != null) blackHoles.TriggerBlackHoles();
                break;
            case 3:
                if (eclipse != null) eclipse.TriggerEclipse();
                break;
            case 4:
                if (primordialSoup != null) primordialSoup.TriggerSoup();
                break;
        }
    }
}