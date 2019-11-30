using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PatternType { Rain, Summon, Claw }

public class BossAI : MonoBehaviour
{
    [SerializeField] private GameObject bossCanvasPrefab;
    [SerializeField] private PatternType pattern;
    private GameObject bossCanvas;

    [Header("Boss Status")]
    [SerializeField] private int maxHealth;
    private int health;

    [Header("Raining Pattern")]
    [SerializeField] private GameObject rainingCircle;
    [SerializeField] private Vector2 rainingPos;

    [Header("Summoning Pattern")]
    [SerializeField] private GameObject summoningCircle;
    [SerializeField] private List<Vector2> summonPos;

    [Header("Claw Pattern")]
    [SerializeField] private GameObject claw;
    [SerializeField] private List<Vector2> clawPos;
    [SerializeField] private Vector2 clawSize;

    [Header("Exit")]
    [SerializeField] private GameObject gatePrefab;
    [SerializeField] private Vector2 exitPos;

    private List<int> patternList = new List<int> { 1 };

    public float HealthRatio { get { return (float)health/maxHealth; } }

    private void Start()
    {
        health = maxHealth;
        bossCanvas = Instantiate(bossCanvasPrefab);
        bossCanvas.GetComponent<BossUIManager>().Boss = this;
        StartCoroutine(NextPattern());
    }

    public void GetDamage(int amount)
    {
        if (amount > 70 && amount < 50) health -= 20;
        else health -= amount;
        StartCoroutine(HitEffect());

        if (health <= 0)
        {
            health = 0;
            StopAllCoroutines();
            StartCoroutine(Dead());
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        for (int i = 0; i < summonPos.Count; i++)
        {
            Gizmos.DrawSphere(summonPos[i] + (Vector2)transform.position, 0.3f);
        }

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(rainingPos + (Vector2)transform.position, 0.3f);

        Gizmos.color = Color.white;
        for (int i = 0; i < clawPos.Count; i++)
        {
            Gizmos.DrawWireCube(clawPos[i] + (Vector2)transform.position, clawSize);
        }

        Gizmos.color = Color.black;
        Gizmos.DrawCube(exitPos, Vector2.one);
    }

    private IEnumerator NextPattern()
    {
        if (patternList.Count == 0) patternList.AddRange(RandomList(3));

        yield return new WaitForSeconds(3f);

        int ran = patternList[0];
        patternList.RemoveAt(0);

        switch (ran)
        {
            case 0:
                StartCoroutine(Summoning());
                break;
            case 1:
                StartCoroutine(Rain());
                break;
            case 2:
                StartCoroutine(Claw());
                break;
            default:
                Debug.LogError("보스 패턴에서 에러가 생겼습니다. 해당하는 패턴을 찾을 수 없습니다.");
                break;
        }
    }

    private IEnumerator Rain()
    {
        // 비를 내리게 하는 오브젝트를 생성
        Instantiate(rainingCircle, rainingPos + (Vector2)transform.position, Quaternion.identity);

        yield return new WaitForSeconds(3f);

        StartCoroutine(NextPattern());
    }

    private IEnumerator Summoning()
    {
        // 지정한 위치에 소환진을 생성
        for (int i = 0; i < summonPos.Count; i++)
        {
            GameObject summoning = Instantiate(summoningCircle, summonPos[i] + (Vector2)transform.position, Quaternion.identity);
            switch (i)
            {
                case 0:
                    summoning.GetComponent<SummonMonster>().MonsterType = 2;
                    break;
                case 1:
                    summoning.GetComponent<SummonMonster>().MonsterType = 2;
                    break;
                case 2:
                    summoning.GetComponent<SummonMonster>().MonsterType = 0;
                    break;
                case 3:
                    summoning.GetComponent<SummonMonster>().MonsterType = 1;
                    break;
            }
        }

        yield return new WaitForSeconds(3f);

        StartCoroutine(NextPattern());
    }

    private IEnumerator Claw()
    {
        int dir = PlayerStatus.instance.transform.position.x >= transform.position.x ? 0 : 1;
        Vector2 attackPos = (dir == 0 ? clawPos[0] : clawPos[1]) + (Vector2)transform.position;

        GameObject nowClaw = Instantiate(claw, attackPos, Quaternion.identity);
        ClawAttack clawAttack = nowClaw.GetComponent<ClawAttack>();
        clawAttack.AttackPos = attackPos;
        clawAttack.AttackSize = clawSize;
        clawAttack.Direction = dir == 0 ? ClawDirection.Right : ClawDirection.Left;

        yield return new WaitForSeconds(3f);

        dir = PlayerStatus.instance.transform.position.x >= transform.position.x ? 0 : 1;
        attackPos = (dir == 0 ? clawPos[0] : clawPos[1]) + (Vector2)transform.position;

        nowClaw = Instantiate(claw, attackPos, Quaternion.identity);
        clawAttack = nowClaw.GetComponent<ClawAttack>();
        clawAttack.AttackPos = attackPos;
        clawAttack.AttackSize = clawSize;
        clawAttack.Direction = dir == 0 ? ClawDirection.Right : ClawDirection.Left;

        yield return new WaitForSeconds(3f);

        StartCoroutine(NextPattern());
    }

    private IEnumerator HitEffect()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        sr.color = Color.red;

        yield return new WaitForSeconds(0.05f);

        sr.color = Color.white;
    }

    private List<int> RandomList(int num)
    {
        List<int> returnList = new List<int>();
        List<int> baseList = new List<int>();

        for (int i = 0; i < num; i++) baseList.Add(i);

        while (baseList.Count != 0)
        {
            int index = Random.Range(0, baseList.Count);
            returnList.Add(baseList[index]);
            baseList.RemoveAt(index);
        }

        return returnList;
    }

    private IEnumerator Dead()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Vector2 originPos = transform.position;
        GameObject gate = Instantiate(gatePrefab, exitPos, Quaternion.identity);

        sr.color = Color.white;
        Destroy(bossCanvas);
        //gate.GetComponent<GateController>().SceneNum = ??

        for (int i = 0; i < 40; i++)
        {
            transform.position = originPos + new Vector2(Random.Range(0, 0.5f), Random.Range(0, 0.5f));
            sr.color -= new Color(0, 0, 0, 0.025f);

            yield return new WaitForSeconds(0.1f);
        }

        Destroy(gameObject);
    }
}
