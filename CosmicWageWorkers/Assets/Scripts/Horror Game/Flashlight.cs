using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class FlashLight : MonoBehaviour
{
    public GameObject fLight;
    public Image batterySprite;
    
    [Header("Battery Visual Settings")]
    [Range(0f, 1f)]
    public float minAlpha = 0.2f; // Minimum opacity at 0% battery
    
    public float batteryLife, maxBatteryLife;
    public float burnCost;
    public bool replaceBattery = false;
    public float batteryRecharge;

    public AudioSource source;
    private PlayerControls controls;
    private bool isFlashlightOn = false;

    void Awake()
    {
        controls = new PlayerControls();
        controls.Gameplay.Flashlight.performed += ctx => ToggleFlash();
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    void Update()
    {
        if (isFlashlightOn)
        {
            batteryLife -= burnCost * Time.deltaTime;
            if (batteryLife <= 0)
            {
                batteryLife = 0;
                replaceBattery = true;
                fLight.GetComponent<Light>().enabled = false;
                isFlashlightOn = false;
            }
        }

        UpdateBatteryVisual();
    }

    private void UpdateBatteryVisual()
    {
        if (batterySprite == null) return;
        
        float batteryPercent = batteryLife / maxBatteryLife;
        
        // Fade alpha from minAlpha to 1.0
        float alpha = Mathf.Lerp(minAlpha, 1f, batteryPercent);
        
        Color color = batterySprite.color;
        color.a = alpha;
        batterySprite.color = color;
    }

    public void ToggleFlash()
    {
        if (!replaceBattery)
        {
            isFlashlightOn = !isFlashlightOn;
            fLight.GetComponent<Light>().enabled = isFlashlightOn;
            SoundEffectManager.Play("Click");
        }
    }
    
    public void ReplenishBattery()
    {
        batteryLife += batteryRecharge;
        if (batteryLife > maxBatteryLife) batteryLife = maxBatteryLife;
        replaceBattery = false;
        
        UpdateBatteryVisual();
        
        SoundEffectManager.Play("BatteryReplace");
    }
}