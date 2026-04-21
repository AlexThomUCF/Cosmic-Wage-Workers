using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Animator transitonAnim;
    [SerializeField] private GameObject transitonObj;
    [SerializeField] public Canvas canvas;
    [SerializeField] private Sprite mainSceneControl;



    public TextMeshProUGUI skipText;

    NPCDialogue dialogueData;
    public static bool isLoading = false;

    private Coroutine flashCoroutine;

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void Awake()
    {
        transitonObj = GameObject.Find("Scene Tansition");

        if (transitonObj != null)
        {
            transitonAnim = transitonObj.GetComponent<Animator>();
            canvas = transitonObj.GetComponentInChildren<Canvas>();
        }


        StartCoroutine(WaitForSkipText());
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(WaitForSkipText());
    }

    private IEnumerator WaitForSkipText()
    {
        float timeout = 2f;
        float timer = 0f;

        while (timer < timeout)
        {
            TextMeshProUGUI[] texts = FindObjectsOfType<TextMeshProUGUI>(true);

            foreach (TextMeshProUGUI text in texts)
            {
                if (text.gameObject.name == "SkipText")
                {
                    skipText = text;
                    skipText.gameObject.SetActive(false);

                    Color color = skipText.color;
                    color.a = 1f;
                    skipText.color = color;

                    Debug.Log("SkipText assigned successfully.");
                    yield break;
                }
            }

            timer += Time.deltaTime;
            yield return null;
        }

        skipText = null;
        Debug.LogWarning("SkipText object not found after waiting.");
    }

    public void LoadSceneByName(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Scene name is empty!");
            return;
        }

        NPC.isInDialogue = false;
        StartCoroutine(LoadLevel(sceneName));
    }

    public void ExitDialogue()
    {
        if (DialogueController.Instance != null)
        {
            DialogueController.Instance.ShowDialogueUI(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Debug.Log("Dialogue closed.");
        }
    }

    public IEnumerator LoadLevel(string sceneName)
    {
        Debug.Log("LoadLevel started");

        
        isLoading = true;

        if (LoadingImageController.Instance != null)
        {
            LoadingImageController.Instance.ShowImage();
        }



        //Start transition animation
        if (transitonAnim != null)
            transitonAnim.SetTrigger("End");

        if (SceneManager.GetActiveScene().name != "MainMenu")
        {
            Debug.Log("Setting sprite");
            LoadingImageController.Instance.SetSprite(mainSceneControl);
        }






        ExitDialogue();

        if (canvas != null)
            canvas.sortingOrder = 2;

       

        // Reset skip text
        if (skipText != null)
        {
            skipText.gameObject.SetActive(false);

            if (flashCoroutine != null)
            {
                StopCoroutine(flashCoroutine);
                flashCoroutine = null;
            }

            Color color = skipText.color;
            color.a = 1f;
            skipText.color = color;
        }

        float timer = 0f;
        float maxWait = 30f;
        float showSkipTime = 4f;

        while (timer < maxWait)
        {
            timer += Time.deltaTime;

            if (timer >= showSkipTime && skipText != null && !skipText.gameObject.activeSelf)
            {
                skipText.gameObject.SetActive(true);
                flashCoroutine = StartCoroutine(FlashSkipText());
            }

            if (timer >= showSkipTime && Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Skipped by player.");
                break;
            }

            yield return null;
        }

        if (flashCoroutine != null)
        {
            StopCoroutine(flashCoroutine);
            flashCoroutine = null;
        }

        if (skipText != null)
        {
            Color color = skipText.color;
            color.a = 1f;
            skipText.color = color;
            skipText.gameObject.SetActive(false);
        }

        // ?? Load scene
        SceneManager.LoadScene(sceneName);

        // ?? End transition animation
        if (transitonAnim != null)
            transitonAnim.SetTrigger("Start");


        // ?? Optional: wait for fade-out animation
        yield return new WaitForSeconds(0.5f);

        if (canvas != null)
            canvas.sortingOrder = -1;

        isLoading = false;

        if (LoadingImageController.Instance != null)
        {
            LoadingImageController.Instance.HideImage();
        }
    }

    private IEnumerator FlashSkipText()
    {
        while (skipText != null && skipText.gameObject.activeSelf)
        {
            float alpha = Mathf.PingPong(Time.time * 2f, 1f);
            Color color = skipText.color;
            color.a = Mathf.Lerp(0.3f, 1f, alpha);
            skipText.color = color;
            

            yield return null;
        }
    }

    public void ExitMiniGame()
    {
        string mainScene = "MainScene";
        StartCoroutine(LoadLevel(mainScene));
    }
}