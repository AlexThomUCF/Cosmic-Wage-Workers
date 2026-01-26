using System.Collections;
using UnityEngine;

public class FinalMiniGame : MonoBehaviour
{
    private static FinalMiniGame Instance;
    public static int miniGameCount = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] SceneLoader loader;
    public void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
            //loader = FindAnyObjectByType<SceneLoader>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(miniGameCount);
        if (miniGameCount == 2)
        {
            miniGameCount = -999; // or any sentinel value
            loader = FindAnyObjectByType<SceneLoader>();
            StartCoroutine(InvasionComing());
            
        }
    }

    IEnumerator InvasionComing()
    {
        Debug.Log("Human Invasion coming");
        yield return new WaitForSeconds(10f);

        string mainSceneName = "FPSMainScene";
        loader.LoadSceneByName(mainSceneName);

    }
}
