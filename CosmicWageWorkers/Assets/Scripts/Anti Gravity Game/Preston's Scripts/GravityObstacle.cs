using UnityEngine;

public class GravityObstacle : MonoBehaviour
{
    public FastFall fastFallScript;
    public GravitySFX gravitySFXScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        gravitySFXScript.clipAudioSource.PlayOneShot(gravitySFXScript.obstacle);
        Physics.gravity = new Vector3(0, -19.62f, 0);
        fastFallScript.fastFallactiviated = true;
        Destroy(gameObject);

    }
}
