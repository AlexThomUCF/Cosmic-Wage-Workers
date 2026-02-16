using UnityEngine;
using TMPro;
using System.Collections;

public class CustomerReaction : MonoBehaviour
{
    [Header("Text Bubble")]
    public TextMeshProUGUI textBubble;
    public Canvas canvas;
    public float displayTime = 2f;

    [Header("Typing Settings")]
    public float typingSpeed = 0.03f; // lower = faster typing

    [Header("Messages")]
    [TextArea]
    public string[] kissLines;

    private Coroutine bubbleRoutine;

    public void ReactToKiss()
    {
        if (kissLines.Length == 0 || textBubble == null) return;

        string message = kissLines[Random.Range(0, kissLines.Length)];

        if (bubbleRoutine != null)
            StopCoroutine(bubbleRoutine);

        bubbleRoutine = StartCoroutine(TypeAndShow(message));
    }

    private IEnumerator TypeAndShow(string message)
    {
        canvas.enabled = true;
        textBubble.text = "";

        // Typewriter effect
        foreach (char letter in message)
        {
            textBubble.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        // Wait before hiding
        yield return new WaitForSeconds(displayTime);

        canvas.enabled = false;
        textBubble.text = "";
    }
}