using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusEffect : MonoBehaviour
{
    [Header("Object Holder")]
    [SerializeField] private GameObject targetIcon;
    [SerializeField] private GameObject buffIcon;

    [Header("Resource Holder")]
    [SerializeField] private List<Sprite> status;
    [SerializeField] private List<Sprite> buff;

    private ResultClass target;
    private bool isBuff;

    private void Start()
    {
        transform.localPosition = isBuff ? new Vector2(0, 1) : new Vector2(0, 2);

        Sprite buffSprite;
        Sprite statusSprite = null;

        if (isBuff) buffSprite = buff[0];
        else buffSprite = buff[1];

        switch (target)
        {
            case ResultClass.MaxHealth:
                statusSprite = status[0];
                break;
            case ResultClass.AttackDamage:
                statusSprite = status[1];
                break;
            case ResultClass.Speed:
                statusSprite = status[2];
                break;
        }

        targetIcon.GetComponent<SpriteRenderer>().sprite = statusSprite;
        buffIcon.GetComponent<SpriteRenderer>().sprite = buffSprite;
    }

    private void FixedUpdate()
    {
        float delta = isBuff ? 1 : -1;
        transform.localPosition += new Vector3(0, delta * Time.fixedDeltaTime);
        if (transform.parent.localScale.x == 1) transform.localScale = Vector3.one;
        else transform.localScale = new Vector3(-1, 1, 1);
    }

    public void Set(ResultClass target, bool isBuff)
    {
        this.target = target;
        this.isBuff = isBuff;
    }
}
