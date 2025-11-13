using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class Dialogue : MonoBehaviour
{
    public Canvas canvas;
    public TextMeshProUGUI textComp;
    public string[] lines;
    public float textSpeed;


    public Button option1;
    public Button option2;

    public string miniGameName;

    private bool hasCompleted = false;
    private int index;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        textComp.text = string.Empty;

        option1.onClick.AddListener(OnButtonClickedYes);
        option2.onClick.AddListener(OnButtonClickedNo);


    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textComp.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComp.text = lines[index];
            }


        }
    }
    public void runDialogue() 
    {
        //Runs if character interact with it
        gameObject.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textComp.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    public void OnButtonClickedYes()
    {
        SceneManager.LoadScene(miniGameName);
    }

    public void OnButtonClickedNo()
    {
        gameObject.SetActive(false);
        //maybe clear text on panel
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComp.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else //make else if click this button aske for mini. 
        {
            //gameObject.SetActive(false);
            //SceneManager.LoadScene(miniGameName);
            //temp load scene
            //Interface loads scene make new script that handles all scene transitions
        }
        //else if click this button exit dialogue or else turn off dialogue
    }
}
