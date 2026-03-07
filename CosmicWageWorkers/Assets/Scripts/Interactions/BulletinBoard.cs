using UnityEngine;

public class BulletinBoard : MonoBehaviour
{
    [Header("UI References")]
    public GameObject bulletinUI; // Reference to BulletinBoardCanvas

    private void Start()
    {
        if (bulletinUI != null)
            bulletinUI.SetActive(true); // Always visible
    }
}

