using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

[RequireComponent(typeof(Animator))]
public class GUN : MonoBehaviour
{
    [SerializeField]
    private bool addBulletSpread = true;

    [SerializeField]
    private Vector3 bulletSpreadVariance = new Vector3(0.1f, 0.1f, 0.1f);

    [SerializeField]
    public ParticleSystem muzzleFlash; //muzzle flash

    [SerializeField] private Transform bulletSpawnPoint;

    [SerializeField]
    private ParticleSystem imapctParticleSystem;

    [SerializeField]
    private TrailRenderer bulletTrail;

    [SerializeField]
    private float shootDelay = 0.5f;

    [SerializeField]
    private LayerMask mask;

    private Animator animator;

    private float lastShootTime;
    public string targetTag = "Enemy";
    public float damage = 20f;

    public bool canMove = false;
    public Camera fpsCam;
    private CinemachineImpulseSource impulseSource;
   


    private PlayerControls inputActions;
    // Start is called once before the first execution of Update after the MonoBehaviour is created


    void Awake()
    {
        inputActions = new PlayerControls();
        animator = GetComponent<Animator>();

        impulseSource = GetComponentInParent<CinemachineImpulseSource>();
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
    public void Update()
    {
        RaycastHit aimCrosshair;
        if(Physics.Raycast(bulletSpawnPoint.transform.position, transform.forward, out aimCrosshair))
        {
            // Debug.Log(aimCrosshair.collider.name);
            if(aimCrosshair.collider.CompareTag("Enemy"))
            {
                canMove = true;
            }
        }
        else
        {
            canMove = false;
        }
    }

    public void Shoot()
    {
        if (lastShootTime + shootDelay < Time.time)
        {
            //use object pool
            animator.SetBool("IsShooting", true);
            SoundEffectManager.Play("Shoot");
            muzzleFlash.Play();
            Vector3 direction = GetDirection();
            CameraShakeManager.instance.CameraShake(impulseSource);

            if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out RaycastHit hit, float.MaxValue, mask))
            {
               
                TrailRenderer trail = Instantiate(bulletTrail, bulletSpawnPoint.transform.position, Quaternion.identity);

                StartCoroutine(SpawnTrail(trail, hit));
                CustomOnCollisionEnter(hit.collider);

                lastShootTime = Time.time;
                Debug.Log(hit.collider);
            }
        }
    }

    public void CustomOnCollisionEnter(Collider collision)
    {
        if (collision.CompareTag(targetTag))
        {
            EnemyBase enemy = collision.GetComponent<EnemyBase>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }

    private Vector3 GetDirection()
    {
        Vector3 direction = transform.forward;
        if (addBulletSpread)
        {
            direction += new Vector3(
                Random.Range(-bulletSpreadVariance.x, bulletSpreadVariance.x),
                Random.Range(-bulletSpreadVariance.y, bulletSpreadVariance.y),
                Random.Range(-bulletSpreadVariance.z, bulletSpreadVariance.z));

            direction.Normalize();
        }

        return direction;

    }

    private IEnumerator SpawnTrail(TrailRenderer trail, RaycastHit hit)
    {
        float time = 0;
        Vector3 startPosition = trail.transform.position;

        while(time < 1)
        {
            trail.transform.position = Vector3.Lerp(startPosition, hit.point, time);// move trail render where it spawns to the hit point over time.

            time += Time.deltaTime / trail.time;
            yield return null;
        }
        animator.SetBool("IsShooting", false);
        trail.transform.position = hit.point;
        Instantiate(imapctParticleSystem, hit.point, Quaternion.LookRotation(hit.normal)); // Impact particle is facing the direction the hit is facing 

        Destroy(trail.gameObject, trail.time);
    }

}
