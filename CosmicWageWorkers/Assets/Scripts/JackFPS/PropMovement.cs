using UnityEngine;
using System.Collections.Generic;

public class PropMovement : MonoBehaviour
{
    public int propPerShelve = 2;
    List<GameObject> props = new List<GameObject>();


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SearchForChild();
        RandomPropMovement();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SearchForChild()
    {
        foreach (Transform child in transform)
        {
            
            if(child.CompareTag("Prop"))
            {
                props.Add(child.gameObject);
                
            }
        }
    }

    public void RandomPropMovement()
    {
        int propListLength = props.Count;
        for (int i = 0; i < propPerShelve; i++)
        {
            int randomInt = Random.Range(0, propListLength);
            GameObject randomProp = props[randomInt];
            Debug.Log("Prop: " + randomProp.name);
            // randomProp.SetActive(false);
            //Play animation
            randomProp.transform.SetParent(null);
            randomProp.tag = "MovingProp";
        }


    }
}
