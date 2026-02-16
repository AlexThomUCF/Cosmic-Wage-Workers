using UnityEngine;
using TMPro;
using System.Collections;

public class CustomerReaction : MonoBehaviour
{
    [Header("Text Bubble")]
    public TextMeshProUGUI textBubble; // assign a world-space canvas TMP text here
    public Canvas canvas;               // world-space canvas parent
    public float displayTime = 2f;      // how long the bubble shows

    [Header("Messages")]
    [TextArea]
    public string[] kissLines;          // messages to display when kissed

    private Coroutine bubbleRoutine;

    public void ReactToKiss()
    {
        if (kissLines.Length == 0 || textBubble == null) return;

        // Pick random line
        string message = kissLines[Random.Range(0, kissLines.Length)];

        // Stop existing routine
        if (bubbleRoutine != null)
            StopCoroutine(bubbleRoutine);

        bubbleRoutine = StartCoroutine(ShowBubble(message));
    }

    private IEnumerator ShowBubble(string message)
    {
        textBubble.text = message;
        canvas.enabled = true;

        yield return new WaitForSeconds(displayTime);

        canvas.enabled = false;
        textBubble.text = "";
    }
}
