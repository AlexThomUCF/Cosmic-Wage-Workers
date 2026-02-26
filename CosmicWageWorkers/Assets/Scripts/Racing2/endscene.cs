using UnityEngine;

public class endscene : MonoBehaviour
{
    public GameObject teen;
    public Camera c1;
    public Camera c2;
    public Animator teenKick;

    public void PlayAnim()
    {
        teen.SetActive(true);

        c1.gameObject.SetActive(false);
        c2.gameObject.SetActive(true);

        teenKick.Play("teen_kicked", 0, 0f);
    }
}