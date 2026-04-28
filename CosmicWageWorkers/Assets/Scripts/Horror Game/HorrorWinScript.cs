using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System.Collections;

public class HorrorWinScript : MonoBehaviour
{
    [Header("Customer Interaction ID")]
    public string interactionID; // Assign the ID of the customer for this minigame
    [SerializeField] SceneLoader loader;
    [SerializeField] private GameObject startCine;
    [SerializeField] private GameObject endCine;
    public GameObject objectTurnOff;

    //[SerializeField] private UnityEvent eventObj;
    
    public void Awake()
    {
        loader = FindAnyObjectByType<SceneLoader>();
        endCine.SetActive(false);
    }

    IEnumerator StartDialogue()
    {
        endCine.SetActive(true);
        startCine.SetActive(false);
        yield return new WaitForSeconds(10f);
        string mainSceneName = "MainScene";
        loader.LoadSceneByName(mainSceneName);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && RealItem.hasItem)
        {
            Debug.Log("Player has won");
            Destroy(objectTurnOff);
            // Mark the interaction complete
            if (!string.IsNullOrEmpty(interactionID))
            {
                CustomerManager.MarkInteractionComplete(interactionID);
            }
            FinalMiniGame.miniGameCount++;
            SaveSystem.SaveGame();
            // Load the main scene
            StartCoroutine(StartDialogue());

            //eventObj.Invoke();

            
           // SceneManager.LoadScene(mainSceneName);
        }
    }
}