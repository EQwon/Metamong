using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGateController : GateController
{
    [SerializeField] private Vector2 bossPos;
    [SerializeField] private GameObject boss;

    public override void UseGate()
    {
        Instantiate(boss, bossPos, Quaternion.identity);
        player.transform.position = exitPos;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(bossPos, Vector2.one * 4);
    }
}
