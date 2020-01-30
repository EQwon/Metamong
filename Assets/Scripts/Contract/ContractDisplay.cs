using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContractDisplay : MonoBehaviour
{
    [SerializeField] private GameObject clausePrefab;

    private List<RectTransform> clauses;

    private void Awake()
    {
        List<SingleContract> contracts = Contract.instance.contracts;
        clauses = new List<RectTransform>();

        for (int i = 0; i < contracts.Count; i++)
        {
            if (contracts[i].Article == 0) continue;

            GameObject clause = CreateClause(contracts[i]);
            clauses.Add(clause.GetComponent<RectTransform>());
        }
    }

    private void Update()
    {
        SizeAjusting();
    }

    private GameObject CreateClause(SingleContract contract)
    {
        GameObject clause = Instantiate(clausePrefab, transform);

        string number;
        if (contract.Clause == 0)
        {
            number = contract.Article + "조";
            RectTransform rect = clause.GetComponent<RectTransform>();
            rect.localPosition += new Vector3(-60f, 0, 0);
            rect.sizeDelta -= new Vector2(0, 50f);
        }
        else number = contract.Clause + "항";

        string text = contract.ContractText;

        clause.name = "Article" + contract.Article + " Clause" + contract.Clause;
        clause.transform.GetChild(0).GetComponent<Text>().text = number;
        clause.transform.GetChild(1).GetComponent<Text>().text = text;
        clause.GetComponent<ContractController>().Init(contract.Article, contract.Clause, contract.RelatedContracts);

        return clause;
    }

    private void SizeAjusting()
    {
        float contentY = 0;

        for (int i = 0; i < clauses.Count; i++)
        {
            float xPos = 0f;
            float ySize = clauses[i].Find("Contract Text").GetComponent<RectTransform>().sizeDelta.y + 35f;

            if (clauses[i].GetComponent<ContractController>().myNum.clause == 0)
            {
                xPos = -60f;
            }
            else
            {
                ySize = 100 > ySize ? 100 : ySize;
            }
            
            clauses[i].sizeDelta = new Vector2(900, ySize);
            clauses[i].anchoredPosition = new Vector2(xPos, contentY);

            contentY -= clauses[i].sizeDelta.y;
        }

        GetComponent<RectTransform>().sizeDelta = new Vector2(GetComponent<RectTransform>().sizeDelta.x, -contentY);
    }
}