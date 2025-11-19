using UnityEngine;

public class AntiGravity : MonoBehaviour
{
    private Rigidbody playerRb;
    public float gravityRandomTimer;
    public bool gravityActive;
    public float gravityDelay;
    public float gravityForce;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerRb = GameObject.Find("MainPlayer").GetComponent<Rigidbody>();
        gravityActive = true;
        gravityDelay = Random.Range(1, 4);

    }

    // Update is called once per frame
    void Update()
    {
        if (!gravityActive)
        {
            gravityRandomTimer -= Time.deltaTime;
            if (gravityRandomTimer > 0)
            {
                Physics.gravity = new Vector3(0, 0, 0);
            }
            if (gravityRandomTimer < 0)
            {
                Physics.gravity = new Vector3(0, -9.81f, 0);
                gravityDelay -= Time.deltaTime;
                if (gravityDelay < 0)
                {
                    gravityRandomTimer = Random.Range(3, 5);
                    gravityDelay = Random.Range(1, 4);
                    playerRb.AddForce(Vector3.up * gravityForce, ForceMode.Impulse);
                }
            }
        }
    }

    public void GravitySwitch()
    {
        gravityActive = !gravityActive;

        if (gravityActive)
        {
            Physics.gravity = new Vector3(0, -9.81f, 0);

        }
        else
        {
            Physics.gravity = new Vector3(0, 0, 0);
            playerRb.AddForce(Vector3.up * gravityForce, ForceMode.Impulse);
            gravityRandomTimer = Random.Range(3, 5);
        }
    }
}