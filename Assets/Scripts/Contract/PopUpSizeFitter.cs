using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpSizeFitter : MonoBehaviour
{
    [SerializeField] private RectTransform warningText;
    [SerializeField] private RectTransform relateContracts;

    private RectTransform myRect;

    private void Update()
    {
        myRect = GetComponent<RectTransform>();

        float height = warningText.sizeDelta.y + relateContracts.sizeDelta.y + 50;
        myRect.sizeDelta = new Vector2(myRect.sizeDelta.x, height);
    }
}
