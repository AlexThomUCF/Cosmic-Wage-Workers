using UnityEngine;

public class ToggleObjects : MonoBehaviour
{
    [Header("References")]
    public GameObject object1;
    public GameObject object2;
    public MopParry mopParry;
    public Animator mopAnimator;

    private bool isAnimating;

    void Update()
    {
        //Change to new unity input system
        //Change to scroll and 1 & 2
        // Press 1 ? enable object1, disable object2
        isAnimating = mopAnimator.GetBool("Swing");

        if (Input.GetKeyDown(KeyCode.Alpha1) && !mopParry.isParrying && !isAnimating)
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
