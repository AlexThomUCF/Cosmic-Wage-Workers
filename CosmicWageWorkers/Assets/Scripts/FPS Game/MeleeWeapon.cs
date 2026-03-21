using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
public class MeleeWeapon : MonoBehaviour
{
    public GameObject rayCastPoint;
    public float rayCastRange;
    public LayerMask mask;

    public float damage = 1f;

    private PlayerControls inputActions;
    private Animator animator;

    public void Awake()
    {
        inputActions = new PlayerControls();
        animator = GetComponent<Animator>();
    }

    void OnEnable()
    {
        inputActions.Gameplay.Enable();
        inputActions.Gameplay.Attack.performed += OnAttack;
        inputActions.Gameplay.Shield.performed += OnShield;
    }

    void OnDisable()
    {
        inputActions.Gameplay.Attack.performed -= OnAttack;
        inputActions.Gameplay.Shield.performed -= OnShield;
        inputActions.Gameplay.Disable();
    }

    private void OnAttack(InputAction.CallbackContext ctx)
    {
        Attack();
    }
    private void OnShield(InputAction.CallbackContext ctx)
    {
        Sheild();
    }


    public void Attack()
    {
        animator.SetBool("Swing", true);
        SoundEffectManager.Play("Swing");
        rayCastRange = 2f;
        StartCoroutine(ResetAttack());
    }

    public void Sheild()
    {
        animator.SetBool("Shield", true);
        SoundEffectManager.Play("Shield");
        StartCoroutine(ResetShield());
    }
    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth player = other.GetComponent<PlayerHealth>();
        if (player != null)
        {
            player.TakeDamage(damage);
        }
    }

    private void Update()
    {
        Debug.DrawRay(rayCastPoint.transform.position, -transform.forward * rayCastRange, Color.red);

        RaycastHit hit;
        if (Physics.Raycast(rayCastPoint.transform.position, -transform.forward, out hit, rayCastRange))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                CustomOnCollisionEnter(hit.collider);
            }
        }
    }

    private IEnumerator ResetAttack()
    {
        yield return new WaitForSeconds(.5f);
        animator.SetBool("Swing", false);
        rayCastRange = 0.2f;
    }
    private IEnumerator ResetShield()
    {
        yield return new WaitForSeconds(1f);
        animator.SetBool("Shield", false);
    }

    public void CustomOnCollisionEnter(Collider collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            EnemyBase enemy = collision.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}


