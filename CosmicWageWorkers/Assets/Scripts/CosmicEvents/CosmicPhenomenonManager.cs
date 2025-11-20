using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmicPhenomenonManager : MonoBehaviour
{
    [Header("Phenomena References")]
    public SolarFlare solarFlare;
    public AntiGravity antiGravity;
    public BlackHoles blackHoles;

    [Header("Timer Settings")]
    public float minInterval = 30f;
    public float maxInterval = 60f;
    private float timer;

    private void Start()
    {
        ResetTimer();
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer <= 0f)
        {
            TriggerRandomPhenomenon();
            ResetTimer();
        }
    }

    private void ResetTimer()
    {
        timer = Random.Range(minInterval, maxInterval);
    }

    private void TriggerRandomPhenomenon()
    {
        int choice = Random.Range(0, 3); // 0 = Solar Flare, 1 = AntiGravity, 2 = Black Holes

        switch (choice)
        {
            case 0:
                if (solarFlare != null)
                    solarFlare.TriggerFlare();
                Debug.Log("Cosmic Phenomenon Triggered: Solar Flare");
                break;

            case 1:
                if (antiGravity != null)
                    antiGravity.TriggerAntiGravity();
                Debug.Log("Cosmic Phenomenon Triggered: AntiGravity");
                break;

            case 2:
                if (blackHoles != null)
                    blackHoles.TriggerBlackHoles();
                Debug.Log("Cosmic Phenomenon Triggered: Black Holes");
                break;
        }
    }
}