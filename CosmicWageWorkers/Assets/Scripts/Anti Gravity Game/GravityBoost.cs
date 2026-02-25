using UnityEngine;

public class GravityBoost : MonoBehaviour
{
    public float boostScale;
    private FastFall fastFallScript;
    private Rigidbody playerRb;
    private GravitySFX gravitySFXScript;
    private RespawnObjects respawnScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        respawnScript = GameObject.Find("RespawnManager").GetComponent<RespawnObjects>();
        playerRb = GameObject.Find("MainPlayer").GetComponent<Rigidbody>();
        gravitySFXScript = GameObject.Find("AudioManager").GetComponent<GravitySFX>();
        fastFallScript = GameObject.Find("MainPlayer").GetComponent<FastFall>();



    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        respawnScript.objectActive = false;
        gravitySFXScript.clipAudioSource.PlayOneShot(gravitySFXScript.boost);
        playerRb.AddForce(Vector3.up * boostScale, ForceMode.Impulse);
        fastFallScript.fastFallactiviated = true;
        Destroy(gameObject);
    }
}
