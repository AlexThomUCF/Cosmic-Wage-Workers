using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.VFX;

public class Kiss : MonoBehaviour
{

    private PlayerControls controls;

    public AudioClip Smooch;
    AudioSource MainPlayer;

    private void Awake()
    {
        controls = new PlayerControls();
        MainPlayer = GetComponent<AudioSource>();
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }
    private void Update()
    {
        if (controls.Gameplay.Kiss.IsPressed())
        {
            Debug.Log("Kiss action triggered!");
            MainPlayer.PlayOneShot(Smooch);
        }
    }
}