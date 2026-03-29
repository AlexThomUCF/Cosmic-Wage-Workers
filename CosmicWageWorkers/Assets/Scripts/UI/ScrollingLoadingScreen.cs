using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollingLoadingScreen : MonoBehaviour
{
    [SerializeField] private RawImage _scroller;
    [SerializeField] private float _x, _y;

    // Update is called once per frame
    void Update()
    {
        _scroller.uvRect = new Rect(_scroller.uvRect.position + new Vector2(_x,_y) * Time.deltaTime,_scroller.uvRect.size);
    }
}
