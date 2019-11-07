using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConditionClass { Always, Kill }
public enum ConditionType { None, Greater, Less }
public enum ResultClass { None, AttackPostDelay, AttackDamage, MaxHealth, Speed, MoveDamping }

[System.Serializable]
public class SingleContract
{
    public int article;
    public int clause;
    public bool isAgree = false;
    public ConditionClass conditionClass;
    public ConditionType conditionType;
    public int conditionValue;
    public ResultClass resultClass;
    public float resultValue;
}

[System.Serializable]
public class Contract : MonoBehaviour
{
    public static Contract instance;

    public List<SingleContract> contracts;

    private PlayerStatus stat;
    private int killCnt = 0;

    public int KillCnt { get { return killCnt; } }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        stat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();
        Initialize();
    }

    private void Initialize()
    {
        for (int i = 0; i < contracts.Count; i++)
        {
            contracts[i].isAgree = false;
        }
    }

    public void ChangeContractState(int article, int clause, bool isAgree)
    {
        for (int i = 0; i < contracts.Count; i++)
        {
            if (contracts[i].article != article) continue;
            if (contracts[i].clause != clause) continue;

            contracts[i].isAgree = isAgree;
            KillContractCheck();
            return;
        }
    }

    public bool GetContractState(int article, int clause)
    {
        for (int i = 0; i < contracts.Count; i++)
        {
            if (contracts[i].article != article) continue;
            if (contracts[i].clause != clause) continue;

            return contracts[i].isAgree;
        }

        Debug.LogError(article + "조 " + clause + "항에 해당하는 계약을 찾지 못했습니다.");
        return false;
    }

    public void KillEvent()
    {
        killCnt += 1;
        Debug.Log("현재 킬 카운트는 " + killCnt + "입니다.");

        KillContractCheck();
    }

    private void KillContractCheck()
    {
        stat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();

        for (int i = 0; i < contracts.Count; i++)
        {
            SingleContract contract = contracts[i];

            if (contract.conditionClass != ConditionClass.Kill) continue;
            if (contract.isAgree == false) continue;

            switch (contract.conditionType)
            {
                case ConditionType.Less:
                    if (killCnt < contract.conditionValue) ActivateResult(i);
                    break;
                case ConditionType.Greater:
                    if (killCnt >= contract.conditionValue) ActivateResult(i);
                    break;
            }
        }

        stat.UpdateStatus();
    }

    private void ActivateResult(int i)
    {
        SingleContract contract = contracts[i];

        switch (contract.resultClass)
        {
            case ResultClass.AttackPostDelay:
                stat.AttackPostDelay = contract.resultValue;
                break;
            case ResultClass.AttackDamage:
                stat.Damage = (int)contract.resultValue;
                break;
            case ResultClass.MaxHealth:
                stat.MaxHealth = (int)contract.resultValue;
                break;
            case ResultClass.Speed:
                stat.Speed = contract.resultValue;
                break;
            case ResultClass.MoveDamping:
                stat.MovementDamping = contract.resultValue;
                break;
        }
    }
}