using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmicPhenomenonManager : MonoBehaviour
{
    [Header("Phenomena References")]
    public BlackHoles blackHole;
    public AntiGravity antiGravity;
    public SolarFlare solarFlare;

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
        int choice = Random.Range(0, 3); // 0,1,2 for the first three phenomena

        switch (choice)
        {
            case 0:
                if (blackHole != null)
                    blackHole.BlackHoleTeleport();
                break;
            case 1:
                if (antiGravity != null)
                    antiGravity.GravitySwitch();
                break;
            case 2:
                if (solarFlare != null)
                    solarFlare.TriggerFlare();
                break;
        }

        Debug.Log($"Cosmic Phenomenon Triggered: {choice}");
    }
}
