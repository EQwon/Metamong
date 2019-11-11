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
    [SerializeField] private GameObject popUp;
    [SerializeField] private Text warningText1;
    [SerializeField] private Text warningText2;
    [SerializeField] private Text relatedContractsText;

    [Header("Info")]
    [SerializeField] private Text killCount;

    [Header("Pause")]
    [SerializeField] private GameObject pausePanel;

    private GameObject player;
    private PlayerInput input;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        input = player.GetComponent<PlayerInput>();
        input.UI = this;

        popUp.SetActive(false);
        pausePanel.SetActive(false);
    }

    private void Update()
    {
        AdjustingHealthBar();
        ShowKillCount();
    }

    private void AdjustingHealthBar()
    {
        Vector3 targetScale = Vector3.one;
        float ratio = (float)input.Health / input.MaxHealth;
        targetScale.x = ratio;
        healthBar.localScale = targetScale;

        healthValue.text = input.Health + " / " + input.MaxHealth;
    }

    public void ShowPopUp(bool willAgree, List<SimpleContract> contracts)
    {
        if (contracts.Count == 0) return;
        popUp.SetActive(true);

        string state = willAgree ? "[동의]" : "[거절]";
        string text1 = "이 조항에 " + state + "할 시";
        string text2 = "";
        string relateContracts = "\n";

        for (int i = 0; i < contracts.Count; i++)
        {
            SingleContract contract = Contract.instance.GetContract(contracts[i].article, contracts[i].clause); ;
            text2 += contract.Article + "조 " + contract.Clause + "항";

            relateContracts += contract.Article + "조 " + contract.Clause + "항\n";
            relateContracts += "- " + contract.ContractText;

            if (i != contracts.Count - 1)
            {
                text2 += ", ";
                relateContracts += "\n";
            }
        }
        text2 += "에 " + state + "하는 것으로 간주합니다.";

        warningText1.text = text1;
        warningText2.text = text2;
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
