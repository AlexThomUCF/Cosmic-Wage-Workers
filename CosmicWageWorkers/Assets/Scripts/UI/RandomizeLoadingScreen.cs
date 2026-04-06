using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomizeLoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject[] _scrollersToRandomize;

    void Start()
    {
        ShowRandomScroller();
    }

    void ShowRandomScroller()
    {
        //Deactivate all scrollers first
        foreach (GameObject img in _scrollersToRandomize)
        {
            img.SetActive(false);
        }

        //Choose a random index (0, 1, or 2)
        int randomIndex = Random.Range(0, _scrollersToRandomize.Length);

        //Activate chosen scroller
        _scrollersToRandomize[randomIndex].SetActive(true);
    }
}
