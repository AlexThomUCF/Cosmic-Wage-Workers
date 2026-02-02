using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public float drainRate = 15f;     // per second
    public float regenRate = 10f;     // per second

    [Header("Lose Settings")]
    public string reloadSceneName = "ShelvesScene";

    [HideInInspector] public Transform lastMovedHand;
    [HideInInspector] public bool stopStamina = false; // <--- new flag to stop drain

    private float leftStamina;
    private float rightStamina;

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
        if (stopStamina) return;  // <-- stop updates when win state

        if (lastMovedHand == null) return;

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

        if (leftStamina <= 0f || rightStamina <= 0f)
        {
            SceneManager.LoadScene(reloadSceneName);
        }
    }
}
