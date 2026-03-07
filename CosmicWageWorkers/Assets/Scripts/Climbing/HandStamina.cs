using UnityEngine;
using UnityEngine.UI;

public class HandStamina : MonoBehaviour
{
    [Header("Hand References")]
    public Transform leftHand;
    public Transform rightHand;

    [Header("UI Sliders")]
    public Slider leftSlider;
    public Slider rightSlider;

    [Header("Settings")]
    public float maxStamina = 100f;
    public float drainRate = 10f;
    public float regenRate = 12f;

    [Header("Lose Settings")]
    public Climbing climbing;

    [HideInInspector] public Transform lastMovedHand;
    [HideInInspector] public bool stopStamina = false;

    private float leftStamina;
    private float rightStamina;
    private bool hasLost = false;

    void Start()
    {
        leftStamina = maxStamina;
        rightStamina = maxStamina;

        leftSlider.maxValue = maxStamina;
        rightSlider.maxValue = maxStamina;

        leftSlider.value = leftStamina;
        rightSlider.value = rightStamina;
    }

    void Update()
    {
        if (stopStamina || hasLost) return;
        if (lastMovedHand == null) return;

        // Drain/regen
        if (lastMovedHand == rightHand)
        {
            rightStamina -= drainRate * Time.deltaTime;
            leftStamina += regenRate * Time.deltaTime;
        }
        else
        {
            leftStamina -= drainRate * Time.deltaTime;
            rightStamina += regenRate * Time.deltaTime;
        }

        leftStamina = Mathf.Clamp(leftStamina, 0f, maxStamina);
        rightStamina = Mathf.Clamp(rightStamina, 0f, maxStamina);

        leftSlider.value = leftStamina;
        rightSlider.value = rightStamina;

        // Trigger lose state
        if ((leftStamina <= 0f || rightStamina <= 0f) && !hasLost)
        {
            hasLost = true;
            stopStamina = true;

            if (climbing != null)
                climbing.TriggerFall();
        }
    }

    public void DamageHand(Transform hand, float amount)
    {
        if (hand == leftHand) leftStamina -= amount;
        else if (hand == rightHand) rightStamina -= amount;
    }
}