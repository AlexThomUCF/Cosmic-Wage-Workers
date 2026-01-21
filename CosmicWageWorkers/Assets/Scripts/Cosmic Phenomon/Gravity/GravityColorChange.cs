using UnityEngine;

public class GravityColorChange : MonoBehaviour
{
  

    private AntiGravity gravity;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gravity = GameObject.Find("NewCosmicManager").GetComponent<AntiGravity>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gravity.gravityRoutineOn)
        {
            GetComponent<Renderer>().material.color = Color.red;
        }
        else
        {
            GetComponent<Renderer>().material.color= Color.green;
        }
    }
}
