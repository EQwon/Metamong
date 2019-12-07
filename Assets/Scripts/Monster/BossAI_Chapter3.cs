using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI_Chapter3 : BossAI
{
    [Header("Spiral Pattern")]
    [SerializeField] private GameObject spiralCircle;
    [SerializeField] private Vector2 spiralPos;

    protected override void DamageArea(int amount)
    {
        if (amount > 75) return;


    }

    protected override IEnumerator Rain()
    {
        int num = Random.Range(0, 100);

        if(num >= 50) Instantiate(rainingCircle, rainingPos + (Vector2)transform.position, Quaternion.identity);
        else Instantiate(spiralCircle, spiralPos + (Vector2)transform.position, Quaternion.identity);

        yield return new WaitForSeconds(5f);

        StartCoroutine(NextPattern());
    }

    protected override IEnumerator Claw()
    {
        int num = Random.Range(0, 100);

        if (num >= 50)
        {
            for (int i = 0; i < clawPos.Count; i++)
            {
                Claw(i);
                yield return new WaitForSeconds(1f);
            }
        }
        else
        {
            for (int i = clawPos.Count - 1; i >= 0 ; i--)
            {
                Claw(i);
                yield return new WaitForSeconds(1f);
            }
        }
        
    }
    private void Claw(int i)
    {
        Vector2 attackPos = clawPos[i] + (Vector2)transform.position;
        GameObject nowClaw = Instantiate(claw, attackPos, Quaternion.identity);
        ClawAttack clawAttack = nowClaw.GetComponent<ClawAttack>();
        clawAttack.AttackPos = attackPos;
        clawAttack.AttackSize = clawSize;
        clawAttack.Direction = (attackPos.x >= transform.position.x) ? ClawDirection.Right : ClawDirection.Left;
    }
}
