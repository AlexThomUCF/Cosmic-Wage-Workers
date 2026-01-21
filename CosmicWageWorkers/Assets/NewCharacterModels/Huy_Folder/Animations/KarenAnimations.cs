using UnityEngine;

public class KarenAnimations : MonoBehaviour
{
    private NPC npc;
    private Animator karenAni;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        npc = GetComponent<NPC>();
        karenAni = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
