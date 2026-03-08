using UnityEngine;

public class TrashBag : MonoBehaviour
{
    private bool isHeld = false;

    public void SetHeld(bool state)
    {
        isHeld = state;
    }

    public bool IsHeld()
    {
        return isHeld;
    }

    public void Dispose()
    {
        Destroy(gameObject);
    }
}