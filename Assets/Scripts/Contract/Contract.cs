using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum ConditionClass { Always, Kill, PlayTime, JumpCnt, AttackCnt }
public enum ConditionType { None, Greater, Less }
public enum ResultClass { None, AttackSpeed, AttackDamage, MaxHealth, Speed, MoveDamping, JumpForce, InvincibleTime, KnockBackForce }

[System.Serializable]
public class SimpleContract
{
    public int article;
    public int clause;

    public SimpleContract(int article, int clause)
    {
        this.article = article;
        this.clause = clause;
    }
}

[System.Serializable]
public class SingleContract
{
    [SerializeField] private int article;
    [SerializeField] private int clause;
    public bool isAgree;
    [SerializeField] private ConditionClass conditionClass;
    [SerializeField] private ConditionType conditionType;
    [SerializeField] private int conditionValue;
    [SerializeField] private ResultClass resultClass;
    [SerializeField] private float resultValue;
    private string contractText;
    private List<SimpleContract> relatedContracts;

    public int Article { get { return article; } }
    public int Clause { get { return clause; } }
    public ConditionClass ConditionClass { get { return conditionClass; } }
    public ConditionType ConditionType { get { return conditionType; } }
    public int ConditionValue { get { return conditionValue; } }
    public ResultClass ResultClass { get { return resultClass; } }
    public float ResultValue { get { return resultValue; } }
    public string ContractText { get { return contractText; } }
    public List<SimpleContract> RelatedContracts { get { return relatedContracts; } }

    public SingleContract(int article, int clause, ConditionClass conditionClass,
                    ConditionType conditionType, int conditionValue, ResultClass resultClass,
                    float resultValue, string contractText, List<SimpleContract> relatedContracts)
    {
        this.article = article;
        this.clause = clause;
        isAgree = false;
        this.conditionClass = conditionClass;
        this.conditionType = conditionType;
        this.conditionValue = conditionValue;
        this.resultClass = resultClass;
        this.resultValue = resultValue;
        this.contractText = contractText;
        this.relatedContracts = relatedContracts;
    }
}

[System.Serializable]
public class Contract : MonoBehaviour
{
    public static Contract instance;

    public List<SingleContract> contracts;

    private int killCnt = 0;

    public int KillCnt { get { return killCnt; } }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(gameObject);

        contracts = new List<SingleContract>();
        contracts = GetComponent<ContractHolder>().ParseContract();
    }

    public SingleContract GetContract(int article, int clause)
    {
        for (int i = 0; i < contracts.Count; i++)
        {
            if (contracts[i].Article != article) continue;
            if (contracts[i].Clause != clause) continue;

            return contracts[i];
        }

        Debug.LogError(article + "조 " + clause + "항에 해당하는 계약을 찾지 못했습니다.");
        return null;
    }

    public void ChangeContractState(int article, int clause, bool isAgree)
    {
        for (int i = 0; i < contracts.Count; i++)
        {
            if (contracts[i].Article != article) continue;
            if (contracts[i].Clause != clause) continue;

            contracts[i].isAgree = isAgree;
            KillContractCheck();
            return;
        }
    }

    public bool GetContractState(int article, int clause)
    {
        for (int i = 0; i < contracts.Count; i++)
        {
            if (contracts[i].Article != article) continue;
            if (contracts[i].Clause != clause) continue;

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

    public void KillContractCheck()
    {
        for (int i = 0; i < contracts.Count; i++)
        {
            SingleContract contract = contracts[i];

            if (contract.ConditionClass != ConditionClass.Kill) continue;
            if (contract.isAgree == false) continue;

            switch (contract.ConditionType)
            {
                case ConditionType.Less:
                    if (killCnt < contract.ConditionValue) ActivateResult(i);
                    break;
                case ConditionType.Greater:
                    if (killCnt >= contract.ConditionValue) ActivateResult(i);
                    break;
            }
        }

        PlayerStatus.instance.UpdateStatus();
    }

    private void ActivateResult(int i)
    {
        SingleContract contract = contracts[i];

        switch (contract.ResultClass)
        {
            case ResultClass.AttackSpeed:
                PlayerStatus.instance.AttackPostDelay = contract.ResultValue;
                break;
            case ResultClass.AttackDamage:
                PlayerStatus.instance.Damage = (int)contract.ResultValue;
                break;
            case ResultClass.MaxHealth:
                PlayerStatus.instance.MaxHealth = (int)contract.ResultValue;
                break;
            case ResultClass.Speed:
                PlayerStatus.instance.Speed = contract.ResultValue;
                break;
            case ResultClass.MoveDamping:
                PlayerStatus.instance.MovementDamping = contract.ResultValue;
                break;
            case ResultClass.JumpForce:
                PlayerStatus.instance.JumpForce = contract.ResultValue;
                break;
            case ResultClass.InvincibleTime:
                PlayerStatus.instance.InvincibleTime = contract.ResultValue;
                break;
            case ResultClass.KnockBackForce:
                PlayerStatus.instance.KnockBackForce = contract.ResultValue;
                break;
            default:
                Debug.LogError("해당하는 결과 클래스를 찾지 못했습니다.");
                break;
        }
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(SimpleContract))]
public class ContractDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(new Rect(position.x, position.y, 30, position.height), property.FindPropertyRelative("article"), GUIContent.none);
        EditorGUI.LabelField(new Rect(position.x + 30, position.y, 15, position.height), "조");
        EditorGUI.PropertyField(new Rect(position.x + 45, position.y, 30, position.height), property.FindPropertyRelative("clause"), GUIContent.none);
        EditorGUI.LabelField(new Rect(position.x + 75, position.y, 15, position.height), "항");

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
#endif