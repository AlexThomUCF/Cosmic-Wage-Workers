using UnityEngine;
using UnityEngine.InputSystem;
public class EnableInputActions : MonoBehaviour
{
    public InputActionReference lookAction;

    void OnEnable()
    {
        lookAction.action.Enable();
    }

    void Update()
    {
        Vector2 delta = lookAction.action.ReadValue<Vector2>();
        if (delta != Vector2.zero)
            Debug.Log("Mouse Delta: " + delta);
    }
}
