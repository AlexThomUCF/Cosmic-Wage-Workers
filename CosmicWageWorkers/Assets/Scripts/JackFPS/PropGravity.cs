using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PropGravity : MonoBehaviour
{
    public int escapedPropCount = 0;
    [SerializeField] private int targetAmount = 10;

    [Header("UI")]
    [SerializeField] private Slider escapeBar;
    [SerializeField] private Image fillImage;
    [SerializeField] private TextMeshProUGUI counterText;

    [Header("Warning Settings")]
    [SerializeField] private float warningThreshold = 0.8f; // 80%
    [SerializeField] private float flashSpeed = 6f;

    private Color normalColor = Color.green;
    private Color warningColor = Color.yellow;
    private Color dangerColor = Color.red;

    void Start()
    {
        if (escapeBar != null)
        {
            escapeBar.maxValue = targetAmount;
            escapeBar.value = 0;
        }

        UpdateUI();
    }

    void Update()
    {
        HandleBarColor();
    }

    void UpdateUI()
    {
        if (escapeBar != null)
            escapeBar.value = escapedPropCount;

        if (counterText != null)
            counterText.text = escapedPropCount + " / " + targetAmount;
    }

    void HandleBarColor()
    {
        if (fillImage == null) return;

        float percent = (float)escapedPropCount / targetAmount;

        if (percent >= warningThreshold)
        {
            // Flash red
            float pulse = Mathf.PingPong(Time.time * flashSpeed, 1f);
            fillImage.color = Color.Lerp(dangerColor, Color.white, pulse);
        }
        else if (percent >= 0.5f)
        {
            fillImage.color = warningColor;
        }
        else
        {
            fillImage.color = normalColor;
        }
    }

    public void PropTracker()
    {
        UpdateUI();

        if (escapedPropCount >= targetAmount)
        {
            SceneManager.LoadScene("JackFPS");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Prop"))
            return;

        // Stop this prop from triggering again
        Collider[] cols = other.GetComponentsInChildren<Collider>();
        foreach (Collider c in cols)
            c.enabled = false;

        Rigidbody rb = other.GetComponent<Rigidbody>();
        if (rb == null)
            rb = other.gameObject.AddComponent<Rigidbody>();

        NavMeshAgent agent = other.GetComponent<NavMeshAgent>();
        if (agent != null)
            agent.enabled = false;

        Vector3 launchDir = transform.TransformDirection(new Vector3(0f, 1f, -1f)).normalized;
        rb.AddForce(launchDir * 3f, ForceMode.Impulse);
        rb.useGravity = false;

        escapedPropCount++;
        PropTracker();

        Destroy(other.gameObject, 1f);
    }
}
