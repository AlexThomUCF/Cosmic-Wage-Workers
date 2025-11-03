using UnityEngine;

public class horrorfootsteps : MonoBehaviour
{
    public AudioSource footsteps;

    void Update()
    {
        if (Input.GetKey(KeyCode.W) || (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || (Input.GetKey(KeyCode.D))))
        {
            footsteps.enabled = true;
        }
        else
        {
            footsteps.enabled = false;
        }
    }
}