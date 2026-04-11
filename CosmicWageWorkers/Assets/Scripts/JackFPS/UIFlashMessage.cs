using UnityEngine;
using TMPro;
using System.Collections;

public class UIFlashMessage : MonoBehaviour
{
    public int flashCount = 4;
    public float flashSpeed = 0.25f;

    private TextMeshProUGUI text;

    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();

        // Keep object ACTIVE, just hide text
        text.enabled = false;
    }

    public void FlashMessage()
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    IEnumerator FlashRoutine()
    {
        for (int i = 0; i < flashCount; i++)
        {
            text.enabled = true;
            yield return new WaitForSeconds(flashSpeed);

            text.enabled = false;
            yield return new WaitForSeconds(flashSpeed);
        }

        // Stay visible briefly at end
        text.enabled = true;
        yield return new WaitForSeconds(1f);

        text.enabled = false;
    }
}