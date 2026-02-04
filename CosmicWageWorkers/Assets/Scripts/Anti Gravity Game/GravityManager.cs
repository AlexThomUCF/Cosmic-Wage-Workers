using UnityEngine;

public class GravityManager : MonoBehaviour
{

    public AntiGravity antiGravityScript;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        antiGravityScript.TriggerAntiGravity();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
