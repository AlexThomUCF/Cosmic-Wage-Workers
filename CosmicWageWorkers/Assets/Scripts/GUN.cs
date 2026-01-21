using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class GUN : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool addBulletSpread = true;
    [SerializeField] private Vector3 bulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);
    [SerializeField] private float shootDelay = 0.5f;
    [SerializeField] private float maxDistance = 300f;
    [SerializeField] private float damage = 20f;
    [SerializeField] private string targetTag = "Enemy";
    [SerializeField] private LayerMask mask;

    [Header("References")]
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private ParticleSystem muzzleFlash;

    private Animator animator;
    private PlayerControls inputActions;
    private float lastShootTime;

    private const string TRAIL_TAG = "Trail";
    private const string IMPACT_TAG = "Impact";

    // --------------------------------------------------------------

    void Awake()
    {
        inputActions = new PlayerControls();
        animator = GetComponent<Animator>();
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

    // --------------------------------------------------------------

    public void Shoot()
    {
        if (Time.time < lastShootTime + shootDelay)
            return;

        lastShootTime = Time.time;

        animator.SetTrigger("Shoot");
        muzzleFlash?.Play();
        SoundEffectManager.Play("Shoot");

        Vector3 direction = GetDirection();

        if (Physics.Raycast(bulletSpawnPoint.position, direction, out RaycastHit hit, maxDistance, mask))
        {
            GameObject trailObj = ObjectPool.Instance.Get(TRAIL_TAG);
            TrailRenderer trail = trailObj.GetComponent<TrailRenderer>();

            trail.transform.position = bulletSpawnPoint.position;
            trail.Clear();

            StartCoroutine(SpawnTrail(trail, hit));
            HandleHit(hit);
        }
    }

    private void HandleHit(RaycastHit hit)
    {
        Collider col = hit.collider;

        if (col.CompareTag(targetTag))
        {
            if (col.TryGetComponent(out EnemyBase enemy))
                enemy.TakeDamage(damage);
        }
    }

    private Vector3 GetDirection()
    {
        Vector3 dir = bulletSpawnPoint.forward;

        if (addBulletSpread)
        {
            dir += new Vector3(
                Random.Range(-bulletSpreadVariance.x, bulletSpreadVariance.x),
                Random.Range(-bulletSpreadVariance.y, bulletSpreadVariance.y),
                Random.Range(-bulletSpreadVariance.z, bulletSpreadVariance.z)
            );
            dir.Normalize();
        }

        return dir;
    }

    // --------------------------------------------------------------

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0f;
        Vector3 start = trail.transform.position;

        while (time < 1f)
        {
            trail.transform.position = Vector3.Lerp(start, hit.point, time);
            time += Time.deltaTime / trail.time;
            yield return null;
        }

        trail.transform.position = hit.point;

        GameObject impactObj = ObjectPool.Instance.Get(IMPACT_TAG);
        impactObj.transform.position = hit.point;
        impactObj.transform.rotation = Quaternion.LookRotation(hit.normal);

        yield return new WaitForSeconds(trail.time);

        trail.gameObject.SetActive(false);
        impactObj.SetActive(false);
    }
}
