using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonMonster : MonoBehaviour
{
    [SerializeField] private List<GameObject> monsterPrefabs;
    [SerializeField] [Range(0, 2)] private int monsterType;

    public int MonsterType { set { monsterType = value; } }

    private void Start()
    {
        StartCoroutine(Summon());
    }

    private IEnumerator Summon()
    {
        yield return new WaitForSeconds(1);

        Instantiate(monsterPrefabs[monsterType], transform.position, Quaternion.identity);
    }
}
