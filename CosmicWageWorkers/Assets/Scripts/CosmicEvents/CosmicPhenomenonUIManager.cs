using UnityEngine;
using UnityEngine.UI;

public class CosmicPhenomenonUIManager : MonoBehaviour
{
    [Header("Event Icons")]
    public Image antiGravityIcon;
    public Image blackHoleIcon;
    public Image eclipseIcon;
    public Image primordialSoupIcon; // new for Primordial Soup

    public void ShowAntiGravity(bool show)
    {
        if (antiGravityIcon != null)
            antiGravityIcon.gameObject.SetActive(show);
    }

    public void ShowBlackHole(bool show)
    {
        if (blackHoleIcon != null)
            blackHoleIcon.gameObject.SetActive(show);
    }

    public void ShowEclipse(bool show)
    {
        if (eclipseIcon != null)
            eclipseIcon.gameObject.SetActive(show);
    }

    public void ShowPrimordialSoup(bool show)
    {
        if (primordialSoupIcon != null)
            primordialSoupIcon.gameObject.SetActive(show);
    }
}
