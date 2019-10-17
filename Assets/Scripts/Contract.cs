using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConditionClass { Always, Kill }
public enum ConditionType { None, Greater, Less }
public enum ResultClass { AttackPostDelay, AttackDamage, MaxHealth, AirControl, Friction }

[System.Serializable]
public class SingleContract
{
    public int article;
    public int clause;
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

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        stat = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatus>();

        KillContractCheck();
    }

    public void KillEvent()
    {
        killCnt += 1;
        Debug.Log("현재 킬 카운트는 " + killCnt + "입니다.");

        KillContractCheck();
    }

    private void KillContractCheck()
    {
        for (int i = 0; i < contracts.Count; i++)
        {
            SingleContract contract = contracts[i];

            if (contract.conditionClass != ConditionClass.Kill) continue;

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
                break;
            case ResultClass.AttackDamage:
                stat.Damage = (int)contract.resultValue;
                break;
            case ResultClass.MaxHealth:
                stat.MaxHealth = (int)contract.resultValue;
                break;
            case ResultClass.AirControl:
                break;
            case ResultClass.Friction:
                break;
        }
    }
}