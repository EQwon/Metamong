using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContractDisplay : MonoBehaviour
{
    [SerializeField] private GameObject clausePrefab;

    private void Start()
    {
        List<SingleContract> contracts = Contract.instance.contracts;

        float validCnt = 0;

        for (int i = 0; i < contracts.Count; i++)
        {
            if (contracts[i].Article == 0) continue;
            if (contracts[i - 1].Article != contracts[i].Article) validCnt += 0.5f;

            GameObject clause = CreateClause(contracts[i]);
            clause.GetComponent<RectTransform>().localPosition -= new Vector3(0, validCnt * 100, 0);
            validCnt += 1;
        }

        GetComponent<RectTransform>().sizeDelta += new Vector2(0, validCnt * 100);
    }

    private GameObject CreateClause(SingleContract contract)
    {
        string number = contract.Article + "항 " + contract.Clause + "조";
        string text = contract.ContractText;

        GameObject clause = Instantiate(clausePrefab, transform);
        clause.name = "Article" + contract.Article + " Clause" + contract.Clause;
        clause.transform.GetChild(0).GetComponent<Text>().text = number;
        clause.transform.GetChild(1).GetComponent<Text>().text = text;
        clause.GetComponent<ContractController>().Init(contract.Article, contract.Clause, contract.RelatedContracts);

        return clause;
    }
}
