using UnityEngine;

public class GunRecoil : MonoBehaviour
{
    public GameObject gun;
    private float recoilAmount = 2f;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //AddRecoil();
    }

    public void AddRecoil()
    {
       gun.transform.Rotate(0,0, recoilAmount + recoilAmount);


    }    
}
