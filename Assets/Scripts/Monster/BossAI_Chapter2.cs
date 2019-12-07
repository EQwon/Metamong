using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI_Chapter2 : BossAI
{
    protected override void DamageArea(int amount)
    {
        if (amount > 75) return;

        if (amount <= 35) health -= amount / 2;
        else health -= amount;

        StartCoroutine(HitEffect());
    }

    protected override IEnumerator Claw()
    {
        ClawPlayer();
        yield return new WaitForSeconds(1f);

        ClawPlayer();
        yield return new WaitForSeconds(1f);

        ClawPlayer();
        yield return new WaitForSeconds(1f);

        StartCoroutine(NextPattern());
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
