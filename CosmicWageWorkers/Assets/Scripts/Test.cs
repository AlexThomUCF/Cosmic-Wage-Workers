using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    [Header("Dialogue Data (read-only)")]
    public NPCDialogue dialogueData;   // just for reference, not modified

    [Header("Scene Settings")]
    public string sceneName;           // type the scene name in Inspector

    [Header("Button Events")]
    public UnityEvent yesClick;
    public UnityEvent onClick;

    void Start()
    {
        // ✅ No dialogueData.onClicks modifications here.
        Debug.Log($"{gameObject.name} initialized with sceneName: '{sceneName}'");
    }

    public void ChangeSceneThing()
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError($"{gameObject.name}: Scene name is empty! Cannot load scene.");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }

    public void DoNothing()
    {
        Debug.Log($"{gameObject.name}: Doing nothing.");
    }
}
