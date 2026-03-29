using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MopParry : MonoBehaviour
{
    public float parryDuration = 0.5f;
    public float parryCooldown = 2f;

    private bool canParry = true;

    public BoxCollider parryBox;
    private PlayerControls inputActions;
    private Animator animator; //Might not need if using sprites and not 3d models. maybe need just to move broom to right
    public ParryLogic parryLogic;  



    public void Awake()
    {
        inputActions = new PlayerControls();
        animator = GetComponent<Animator>();
        parryBox.enabled = false;
    }

    void OnEnable()
    {
        inputActions.Gameplay.Enable();
        inputActions.Gameplay.Parry.performed += OnParry;
    }

    void OnDisable()
    {
        inputActions.Gameplay.Parry.performed -= OnParry;
        inputActions.Gameplay.Disable();
    }

   
    private void OnParry(InputAction.CallbackContext ctx)
    {
        Parry();
    }

    private void Parry()
    {
        if (!canParry) return;

        canParry = false;
        parryBox.enabled = true;

        StartCoroutine(ParryRoutine());
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    IEnumerator ParryRoutine()
    {
        float timer = 0f;

        // Parry active window
        while (timer < parryDuration)
        {
            if (parryLogic.IsMaxed())
                break;

            timer += Time.deltaTime;
            yield return null;
        }

        // Turn off parry hitbox
        parryBox.enabled = false;

        // Reset UI + counter
        parryLogic.ResetParryState();

        // Cooldown
        yield return new WaitForSeconds(parryCooldown);

        canParry = true;
    }

}
