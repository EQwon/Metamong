using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContractHolder : MonoBehaviour
{
    [SerializeField] private TextAsset contractAsset;

    public List<SingleContract> ParseContract()
    {
        List<SingleContract> ret = new List<SingleContract>();

        List<List<string>> data = Parser.ContractParse(contractAsset);

        for (int i = 0; i < data.Count; i++)
        {
            int article = int.Parse(data[i][0]);
            int clause = int.Parse(data[i][1]);
            ConditionClass conditionClass = (ConditionClass)System.Enum.Parse(typeof(ConditionClass), data[i][2]);
            ConditionType conditionType = (ConditionType)System.Enum.Parse(typeof(ConditionType), data[i][3]);
            int conditionValue = int.Parse(data[i][4]);
            ResultClass resultClass = (ResultClass)System.Enum.Parse(typeof(ResultClass), data[i][5]);
            float resultValue = float.Parse(data[i][6]);
            string contractText = data[i][7];
            List<SimpleContract> relatedContracts = new List<SimpleContract>();

            if (data[i].Count > 8)
            {
                for (int j = 8; j < data[i].Count; j++)
                {
                    if (data[i][j] == "") continue;                    
                    int relatedContract = int.Parse(data[i][j]);
                    int relatedArticle = relatedContract / 10;
                    int relatedClause = relatedContract % 10;

                    SimpleContract cont = new SimpleContract(relatedArticle, relatedClause);
                    relatedContracts.Add(cont);
                }
            }

            SingleContract contract = new SingleContract(article, clause, conditionClass, conditionType, conditionValue, resultClass, resultValue, contractText, relatedContracts);
            if (contract.Article == 0) contract.isAgree = true;
            ret.Add(contract);
        }

        return ret;
    }
}