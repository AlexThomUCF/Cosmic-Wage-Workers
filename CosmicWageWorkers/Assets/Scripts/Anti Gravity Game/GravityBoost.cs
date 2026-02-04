using UnityEngine;

public class GravityBoost : MonoBehaviour
{
    public float boostScale;
    public FastFall fastFallScript;
    public Rigidbody playerRb;
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
        playerRb.AddForce(Vector3.up * boostScale, ForceMode.Impulse);
        fastFallScript.fastFallactiviated = true;
        Destroy(gameObject);
    }
}
