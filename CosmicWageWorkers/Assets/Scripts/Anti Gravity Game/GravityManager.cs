using UnityEngine;

public class GravityManager : MonoBehaviour
{

    public AntiGravity antiGravityScript;
    public float gravityScale = -1.0f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Physics.gravity = new Vector3(0, gravityScale, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
