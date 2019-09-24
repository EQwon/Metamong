using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleBlockController : MonoBehaviour
{
    public int myNumber;
    private Vector3 originPos;
    private RectTransform rect;

    private void Start()
    {
        rect = GetComponent<RectTransform>();
        originPos = rect.position;
    }

    public void FollowMouse()
    {
        Debug.Log(Input.mousePosition);
        
        rect.localScale = new Vector3(1.5f, 1.5f, 1f);
        rect.position = (Vector2)Input.mousePosition;
    }

    public void BackToOrigin()
    {
        rect.localScale = Vector3.one;
        rect.position = originPos;
    }
}
