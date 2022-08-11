using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BGScroller : MonoBehaviour
{
    public RawImage bg;
    public float x, y;

    private void Update()
    {
        bg.uvRect = new Rect(bg.uvRect.position + new Vector2(x, y) * Time.deltaTime, bg.uvRect.size);
    }
}
