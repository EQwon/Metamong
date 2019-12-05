using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGateController : GateController
{
    [SerializeField] private Vector2 bossPos;
    [SerializeField] private GameObject boss;
    [SerializeField] private Vector2 bossRoomExitPos;

    public override void UseGate()
    {
        GameObject nowBoss = Instantiate(boss, bossPos, Quaternion.identity);
        nowBoss.GetComponent<BossAI>().ExitPos = bossRoomExitPos;

        player.transform.position = exitPos;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(bossPos, Vector2.one * 4);

        Gizmos.color = Color.black;
        Gizmos.DrawCube(bossRoomExitPos, Vector2.one);
    }
}
