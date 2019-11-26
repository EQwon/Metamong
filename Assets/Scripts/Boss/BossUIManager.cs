using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossUIManager : MonoBehaviour
{
    [SerializeField] private RectTransform healthBar;

    private BossAI boss;
    private float originSize;

    public BossAI Boss { set { boss = value; } }

    private void Start()
    {
        originSize = healthBar.sizeDelta.x;
    }

    private void Update()
    {
        float ratio = boss.HealthRatio;
        healthBar.sizeDelta = new Vector2(originSize * ratio, healthBar.sizeDelta.y);
    }
}
