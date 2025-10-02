using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    public Camera cam;
    public float shootRange = 50f;
    public float damage = 10f;
    public LayerMask enemyLayer;

    private PlayerControls inputActions;

    void Awake()
    {
        inputActions = new PlayerControls();
    }

    void OnEnable()
    {
        inputActions.Gameplay.Enable();
        inputActions.Gameplay.Shoot.performed += OnShoot;
    }

    void OnDisable()
    {
        inputActions.Gameplay.Shoot.performed -= OnShoot;
        inputActions.Gameplay.Disable();
    }

    private void OnShoot(InputAction.CallbackContext ctx)
    {
        Shoot();
    }

    void Shoot()
    {
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out RaycastHit hit, shootRange, enemyLayer))
        {
            EnemyBase enemy = hit.collider.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }
}

