using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractEffectController : MonoBehaviour
{
    private void Start()
    {
        transform.localPosition = new Vector2(0, 1.3f);
    }

    private void FixedUpdate()
    {
        transform.localScale = transform.parent.localScale.x == 1 ? Vector3.one : new Vector3(-1, 1, 1);
    }
}
