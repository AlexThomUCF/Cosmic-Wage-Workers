using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneTracker : MonoBehaviour
{
    public static SceneTracker Instance;
    public string previousScene;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        previousScene = SceneManager.GetActiveScene().name;
    }
}