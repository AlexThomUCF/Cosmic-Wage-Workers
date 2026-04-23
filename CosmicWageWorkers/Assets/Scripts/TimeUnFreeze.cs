using UnityEngine;

public class TimeUnFreeze : MonoBehaviour
{
    public void Awake()
    {
        Time.timeScale = 1.0f;
    }
}
