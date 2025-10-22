using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;
using TMPro;

public class GeneralUI : MonoBehaviour
{
    public TextMeshProUGUI boxText;
    public TextMeshProUGUI customerText;
    public float boxCount;
    public float customerCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateBoxes();
        UpdateCustomers();
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

    public void UpdateCustomers()
    {
        customerCount++;
        customerText.text = customerCount + " / 10";
    }
}
