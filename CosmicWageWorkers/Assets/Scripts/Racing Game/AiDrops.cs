using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AiDrops : MonoBehaviour
{
    public float slowSpeed; // how slow the vehicle will slow down

    [Header("UI Feedback")]
    public Image hitImage;
    public float imageDuration = 1f;

    public KartControllerArcade car;
    public KartControllerArcade currentCar;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        KartControllerArcade kart = other.GetComponentInParent<KartControllerArcade>();

        if (kart != null)
        {
            kart.ApplySlow(slowSpeed, 2f);

            if (hitImage == null)
            {
                GameObject obj = GameObject.Find("HitImage");
                if (obj != null)
                    hitImage = obj.GetComponent<Image>();
            }

            if (hitImage != null)
            {
                StartCoroutine(ShowHitImage());
            }

            //stroy(gameObject);
        }
    }
    private void Awake()
    {
        GameObject obj = GameObject.Find("poo");
        hitImage = obj.GetComponent<Image>();
    }
    IEnumerator ShowHitImage()
    {
        
        hitImage.enabled = true;
        yield return new WaitForSeconds(imageDuration);
        hitImage.enabled = false;
    }
}
