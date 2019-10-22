using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private RectTransform healthBar;

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        Vector3 targetScale = Vector3.one;
        targetScale.x = player.GetComponent<PlayerInput>().HealthRatio;
        healthBar.localScale = targetScale;
    }
}
