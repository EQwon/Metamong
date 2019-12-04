using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI_Chapter2 : BossAI
{
    protected override IEnumerator Claw()
    {
        ClawPlayer();
        yield return new WaitForSeconds(1f);

        ClawPlayer();
        yield return new WaitForSeconds(1f);

        ClawPlayer();
        yield return new WaitForSeconds(4f);

        NextPattern();
    }

    private void ClawPlayer()
    {
        Vector2 attackPos = PlayerStatus.instance.transform.position;
        GameObject nowClaw = Instantiate(claw, attackPos, Quaternion.identity);
        ClawAttack clawAttack = nowClaw.GetComponent<ClawAttack>();
        clawAttack.AttackPos = attackPos;
        clawAttack.AttackSize = clawSize;
        clawAttack.Direction = (attackPos.x >= transform.position.x) ? ClawDirection.Right : ClawDirection.Left;
    }
}
