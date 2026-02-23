using UnityEngine;

public class ScaleIncrease : MonoBehaviour
{
    public float scaleTimer;
    public float normalTimer;
    private bool isIncreasing = false;
    private bool isNormal = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isIncreasing)
        {
            transform.localScale += new Vector3(0.01f, 0.03f, 0.03f);
            scaleTimer -= Time.deltaTime;
            if (scaleTimer <= 0f && isIncreasing)
            {
                transform.localScale += new Vector3(0.01f, 0.01f, 0.01f);
                scaleTimer = 4f;
                isIncreasing = false;
                isNormal = true;
            }
        }
        if (isNormal)
        {
            normalTimer -= Time.deltaTime;
            if (normalTimer <= 0f && isNormal)
            {
                transform.localScale = new Vector3(0.03f, 0.03f, 0.03f);
                normalTimer = 4f;
                isNormal = false;
                isIncreasing = true;
            }
        }
    }
}
