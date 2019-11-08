using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContractController : MonoBehaviour
{
    [SerializeField] private SimpleContract myNum;
    [SerializeField] private List<SimpleContract> relatedContracts;

    [SerializeField] private Image box;
    [SerializeField] private List<Sprite> checkImage;

    private void Update()
    {
        if (Contract.instance.GetContractState(myNum.article, myNum.clause)) box.sprite = checkImage[1];
        else box.sprite = checkImage[0];
    }

    public void Init(int article, int clause, List<SimpleContract> relates)
    {
        myNum.article = article;
        myNum.clause = clause;
        relatedContracts = relates;
    }

    public void CheckBox()
    {
        if (Contract.instance.GetContractState(myNum.article, myNum.clause)) Disagree();
        else Agree();
    }

    private void Agree()
    {
        Contract.instance.ChangeContractState(myNum.article, myNum.clause, true);

        for (int i = 0; i < relatedContracts.Count; i++)
        {
            SimpleContract groupNum = relatedContracts[i];
            Contract.instance.ChangeContractState(groupNum.article, groupNum.clause, true);
        }
    }

    private void Disagree()
    {
        Contract.instance.ChangeContractState(myNum.article, myNum.clause, false);

        for (int i = 0; i < relatedContracts.Count; i++)
        {
            SimpleContract groupNum = relatedContracts[i];
            Contract.instance.ChangeContractState(groupNum.article, groupNum.clause, false);
        }
    }
}