using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum ContractType { Manatory, Optional }

[System.Serializable]
public struct ContractNum
{
    public int article;
    public int clause;
}

public class ContractController : MonoBehaviour
{
    [SerializeField] private ContractType type;
    [SerializeField] private ContractNum myNum;
    [SerializeField] private ContractNum penaltyNum;

    [SerializeField] private Image box;

    private void Start()
    {
        Agree();
    }

    private void Update()
    {
        if (Contract.instance.GetContractState(myNum.article, myNum.clause)) box.color = Color.black;
        else box.color = Color.white;
    }

    public void CheckBox()
    {
        if (type == ContractType.Manatory) return;

        if (Contract.instance.GetContractState(myNum.article, myNum.clause)) Disagree();
        else Agree();
    }

    private void Agree()
    {
        Contract.instance.ChangeContractState(myNum.article, myNum.clause, true);
        box.color = Color.black;
    }

    private void Disagree()
    {
        Contract.instance.ChangeContractState(myNum.article, myNum.clause, false);

        if (type == ContractType.Manatory) return;

        Contract.instance.ChangeContractState(penaltyNum.article, penaltyNum.clause, true);
    }
}
