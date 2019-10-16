using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack_Rush : MonsterAttack
{
    [Header("Rush")]
    [SerializeField] private float rushSpeed = 8f;

    private MonsterMovement mover;

    private void Start()
    {
        mover = GetComponent<MonsterMovement>();
    }

    public override void DoAttack()
    {
        base.DoAttack();
        mover.Rush(rushSpeed);
    }
}
