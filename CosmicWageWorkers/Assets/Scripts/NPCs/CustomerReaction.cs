using UnityEngine;
using TMPro;
using System.Collections;

public class CustomerReaction : MonoBehaviour
{
    [Header("Bubble References")]
    public GameObject bubbleRoot;              // The speech bubble root (background + text)
    public TextMeshProUGUI textBubble;         // The TMP text inside the bubble

    [Header("Timing")]
    public float typingSpeed = 0.03f;          // Lower = faster typing
    public float displayTime = 2f;             // How long bubble stays after typing

    [Header("Messages")]
    [TextArea]
    public string[] kissLines;

    private Coroutine bubbleRoutine;

    private void Start()
    {
        if (bubbleRoot != null)
            bubbleRoot.SetActive(false);
    }

    public void ReactToKiss()
    {
        if (kissLines == null || kissLines.Length == 0) return;
        if (textBubble == null || bubbleRoot == null) return;

        string message = kissLines[Random.Range(0, kissLines.Length)];

        if (bubbleRoutine != null)
            StopCoroutine(bubbleRoutine);

        bubbleRoutine = StartCoroutine(TypeAndShow(message));
    }

    private IEnumerator TypeAndShow(string message)
    {
        bubbleRoot.SetActive(true);
        textBubble.text = "";

        // Typewriter effect
        foreach (char letter in message)
        {
            textBubble.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        // Wait after full text appears
        yield return new WaitForSeconds(displayTime);

        bubbleRoot.SetActive(false);
        textBubble.text = "";
    }
}