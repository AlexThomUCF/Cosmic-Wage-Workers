using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooting : MonoBehaviour
{
    public Camera cam;
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public GameObject gun;
    public float projectileSpeed = 25f;
    public float recoilAmount = 9f;



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
        if (projectilePrefab != null && shootPoint != null)
        {
            SoundEffectManager.Play("Shoot");
            GameObject proj = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
            Rigidbody rb = proj.GetComponent<Rigidbody>();
            rb.linearVelocity = cam.transform.forward * projectileSpeed;
        }
        StartCoroutine(Recoil());
    }

    IEnumerator Recoil()
    {
        float recoilAngle = Mathf.Clamp(recoilAmount, 0, 12);

        // Apply recoil (rotate backward)
        gun.transform.Rotate(0, 0, recoilAngle);

        // Wait
        yield return new WaitForSeconds(0.1f); // shorter time for recoil

        // Return to original rotation
        gun.transform.Rotate(0, 0, -recoilAngle);
    }

}