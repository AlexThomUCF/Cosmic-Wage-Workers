using UnityEngine;
using UnityEngine.SceneManagement;

public class HorrorWinScript : MonoBehaviour
{
    
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" && RealItem.hasItem)
        {
            Debug.Log("Player has won");

            string mainSceneName = "POCScene";

            SceneManager.LoadScene(mainSceneName);
        }
    }
}
