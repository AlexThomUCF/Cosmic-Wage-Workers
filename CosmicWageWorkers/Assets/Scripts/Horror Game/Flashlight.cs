using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class FlashLight : MonoBehaviour
{
    public GameObject fLight;
    public Image batteryBar;

    public float batteryLife, maxBatteryLife;
    public float burnCost;
    public bool replaceBattery = false;
    public float batteryRecharge;

    public AudioSource source;
    private PlayerControls controls;
    //private Battery battery;
    private bool isFlashlightOn = false; // Track flashlight state

    void Awake()
    {
        //battery = FindObjectOfType<Battery>();
        controls = new PlayerControls();

        // Bind flashlight toggle event
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
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            ToggleFlash();
        }

        // Reduce battery only if flashlight is on
        if (isFlashlightOn)
        {
            batteryLife -= burnCost * Time.deltaTime;
            if (batteryLife <= 0)
            {
                batteryLife = 0;
                replaceBattery = true;
                fLight.SetActive(false);
                isFlashlightOn = false;
            }
        }

        batteryBar.fillAmount = batteryLife / maxBatteryLife;

        // ReplenishBattery();
    }

    public void ToggleFlash()
    {
        if (!replaceBattery)
        {
            isFlashlightOn = !isFlashlightOn;
            fLight.SetActive(isFlashlightOn);
            source.Play();
        }
    }
    /*public void ReplenishBattery()
    {
        if(battery.newBattery == true && batteryLife < maxBatteryLife)
        {
            
            batteryLife += batteryRecharge;
            if(batteryLife > maxBatteryLife) batteryLife = maxBatteryLife;
            batteryBar.fillAmount = batteryLife;
            replaceBattery = false;
            battery.newBattery = false;

        }
    
    }*/
}