using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PatternType { Rain, Summon, Axe }

public class BossAI : MonoBehaviour
{
    [SerializeField] private PatternType pattern;

    [Header("Raining Pattern")]
    [SerializeField] private GameObject rainingCircle;
    [SerializeField] private Vector2 rainingPos;

    [Header("Summoning Pattern")]
    [SerializeField] private GameObject summoningCircle;
    [SerializeField] private List<Vector2> summonPos;

    private void Start()
    {
        StartCoroutine(NextPattern());
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
    }

    private IEnumerator NextPattern()
    {
        yield return new WaitForSeconds(3f);

        int ran = Random.Range(0, 100);
        switch (ran % 2)
        {
            case 0:
                StartCoroutine(Summoning());
                break;
            case 1:
                StartCoroutine(Rain());
                break;
        }
    }

    private IEnumerator Rain()
    {
        // 비를 내리게 하는 오브젝트를 생성
        Instantiate(rainingCircle, rainingPos + (Vector2)transform.position, Quaternion.identity);

        yield return new WaitForSeconds(8f);

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

        yield return new WaitForSeconds(4f);

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
                    summoning.GetComponent<SummonMonster>().MonsterType = 1;
                    break;
                case 3:
                    summoning.GetComponent<SummonMonster>().MonsterType = 0;
                    break;
            }
        }

        yield return new WaitForSeconds(4f);

        StartCoroutine(NextPattern());
    }
}
