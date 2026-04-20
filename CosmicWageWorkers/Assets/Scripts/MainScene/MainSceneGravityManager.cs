using UnityEngine;

public class MainSceneGravityManager : MonoBehaviour
{
    void Start()
    {
        // If this is the first time, store the gravity
        if (GravityStore.MainSceneGravity == Vector3.zero)
        {
            GravityStore.MainSceneGravity = Physics.gravity;
        }

        // Always reset to MainScene gravity
        Physics.gravity = GravityStore.MainSceneGravity;
    }
}