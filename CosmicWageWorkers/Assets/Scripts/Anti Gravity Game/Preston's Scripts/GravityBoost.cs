using UnityEngine;

public class GravityBoost : MonoBehaviour
{
    public float boostScale;
    private FastFall fastFallScript;
    private Rigidbody playerRb;
    private GravitySFX gravitySFXScript;
    public string respawnName;
    private RespawnObjects respawnScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRb = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody>();
        fastFallScript = GameObject.FindGameObjectWithTag("Player").GetComponent<FastFall>();
        gravitySFXScript = GameObject.Find("AudioManager").GetComponent<GravitySFX>();
        respawnScript = GameObject.Find(respawnName).GetComponent<RespawnObjects>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        gravitySFXScript.clipAudioSource.PlayOneShot(gravitySFXScript.boost);
        playerRb.AddForce(Vector3.up * boostScale, ForceMode.Impulse);
        fastFallScript.fastFallactiviated = true;
        respawnScript.objectActive = false;
        Destroy(gameObject);

    }
}
