using UnityEngine;

public class ToggleObjects : MonoBehaviour
{
    public GameObject object1;
    public GameObject object2;

    void Update()
    {
        // Press 1 ? enable object1, disable object2
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            object1.SetActive(true);
            object2.SetActive(false);
        }

        // Press 2 ? enable object2, disable object1
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            object1.SetActive(false);
            object2.SetActive(true);
        }
    }
}
