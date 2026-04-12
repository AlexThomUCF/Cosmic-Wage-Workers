using System.Collections;
using UnityEngine;

public class OpenDoor : MonoBehaviour
{
    public GameObject promptUI;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisonEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            promptUI.SetActive(true);
        }
    }



}
