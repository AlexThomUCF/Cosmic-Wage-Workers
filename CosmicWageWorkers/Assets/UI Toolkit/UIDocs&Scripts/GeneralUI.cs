using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using TMPro;

public class GeneralUI : MonoBehaviour
{
    public TextMeshProUGUI boxText;
    public float boxCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateBoxes();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void UpdateBoxes()
    {
        boxCount++;
        boxText.text = boxCount + " / 10";
    }
}
