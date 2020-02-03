using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatusEffect : MonoBehaviour
{
    private Text myText;
    private float value;
    private ResultClass target;
    private bool isBuff;
    private RectTransform rect;

    private void Start()
    {
        myText = GetComponent<Text>();
        rect = GetComponent<RectTransform>();

        rect.localPosition = new Vector2(0, 250);

        string buffText;
        string statusText;

        if (value > 0) buffText = " +";
        else buffText = " -";

        switch (target)
        {
            case ResultClass.MaxHealth:
                statusText = "Max.HP ";
                break;
            case ResultClass.AttackDamage:
                statusText = "Attack Damage ";
                break;
            case ResultClass.AttackSpeed:
                statusText = "Attack Speed ";
                break;
            case ResultClass.Speed:
                statusText = "Movement Speed ";
                break;
            case ResultClass.BossHealth:
                statusText = "Boss Hp ";
                break;
            default:
                statusText = "버그남 ";
                break;
        }

        myText.text = statusText + buffText + Mathf.Abs(value).ToString();
    }

    private void FixedUpdate()
    {
        float delta = isBuff ? 50 : -50;
        rect.localPosition += new Vector3(0, delta * Time.fixedDeltaTime);
        if (transform.parent.localScale.x == 1) rect.localScale = Vector3.one;
        else rect.localScale = new Vector3(-1, 1, 1);
    }

    public void Set(ResultClass target, float value, bool isBuff)
    {
        this.target = target;
        this.value = value;
        this.isBuff = isBuff;
    }
}
