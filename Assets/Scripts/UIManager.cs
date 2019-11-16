using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Health Bar")]
    [SerializeField] private RectTransform healthBar;
    [SerializeField] private Text healthValue;

    [Header("Contract")]
    [SerializeField] private GameObject contractPanel;
    [SerializeField] private GameObject contractConfirmPanel;
    [SerializeField] private GameObject popUp;
    [SerializeField] private Text warningText;
    [SerializeField] private Text relatedContractsText;
    [SerializeField] private bool canChangeContract = false;

    [Header("Info")]
    [SerializeField] private Text killCount;

    [Header("Pause")]
    [SerializeField] private GameObject pausePanel;

    public bool CanChangeContract { get { return canChangeContract; } set { canChangeContract = value; } }

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        contractPanel.SetActive(false);
        contractConfirmPanel.SetActive(false);
        popUp.SetActive(false);
        pausePanel.SetActive(false);
    }

    private void Update()
    {
        ShowKillCount();
    }

    public void AdjustingHealthBar(int health, int maxHealth)
    {
        Vector3 targetScale = Vector3.one;
        float ratio = (float)health / maxHealth;
        targetScale.x = ratio;
        healthBar.localScale = targetScale;

        healthValue.text = health + " / " + maxHealth;
    }

    public void OpenContract()
    {
        contractPanel.SetActive(true);
    }

    public void CloseContract()
    {
        if (canChangeContract)
        {
            contractConfirmPanel.SetActive(true);
        }
        else
        {
            contractPanel.SetActive(false);
            contractConfirmPanel.SetActive(false);
        }
    }

    public void ShowPopUp(bool willAgree, List<SimpleContract> contracts)
    {
        if (contracts.Count == 0) return;
        popUp.SetActive(true);

        string state = willAgree ? "<color=#0000ffff>[동의]</color>" : "<color=#ff0000ff>[거절]</color>";
        string text = "이 조항에 " + state + "할 시 ";
        string relateContracts = "\n";

        for (int i = 0; i < contracts.Count; i++)
        {
            SingleContract contract = Contract.instance.GetContract(contracts[i].article, contracts[i].clause); ;
            text += contract.Article + "조 " + contract.Clause + "항";

            relateContracts += contract.Article + "조 " + contract.Clause + "항\n";
            relateContracts += "- " + contract.ContractText;

            if (i != contracts.Count - 1)
            {
                text += ", ";
                relateContracts += "\n";
            }
        }
        text += "에 " + state + "하는 것으로 간주합니다.";

        warningText.text = text;
        relatedContractsText.text = relateContracts;
    }

    public void HidePopUp()
    {
        popUp.SetActive(false);
    }

    private void ShowKillCount()
    {
        killCount.text = Contract.instance.KillCnt.ToString();
    }

    public void MoveToTitle()
    {
        SceneManager.LoadScene(0);
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
    }
}
