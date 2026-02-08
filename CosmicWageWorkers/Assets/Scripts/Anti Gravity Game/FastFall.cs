using UnityEngine;

public class FastFall : MonoBehaviour
{
    public float fallTime = 1.0f;
    public bool fastFallactiviated = false;
    public GravityManager gravityManager;
    public GravitySFX gravitySFXScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Physics.gravity = new Vector3(0, -19.62f, 0); // double normal gravity
            gravitySFXScript.clipAudioSource.PlayOneShot(gravitySFXScript.fastFall);
            fastFallactiviated = true;
        }

        if (fastFallactiviated)
        {
            fallTime -= Time.deltaTime;
            if (fallTime <= 0f)
            {
                Physics.gravity = new Vector3(0, gravityManager.gravityScale, 0); // restore normal gravity
                fallTime = 0.7f;
                fastFallactiviated = false;
            }
        }
    }
}
