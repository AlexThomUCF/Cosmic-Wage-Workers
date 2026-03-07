using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ScaleIncrease : MonoBehaviour
{
    public float a;
    public float b;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        a = Mathf.Sin(Time.time);
        transform.localScale = Vector3.one * (Mathf.Sin(Time.time * a) + b);
    }
}
