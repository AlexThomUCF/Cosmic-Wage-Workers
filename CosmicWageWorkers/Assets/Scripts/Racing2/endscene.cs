using UnityEngine;

public class endscene : MonoBehaviour
{
    public GameObject teen;
    public GameObject origin_teen;
    public Camera c1;
    public Camera c2;
    

    public void PlayAnim()
    {
        teen.SetActive(true);
        origin_teen.SetActive(false);

        c1.gameObject.SetActive(false);
        c2.gameObject.SetActive(true);
        
    }
}