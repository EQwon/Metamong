﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    [Header("Health Bar")]
    [SerializeField] private Image healthBar;
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
    [SerializeField] private Text attackDamageValueText;
    [SerializeField] private Text attackSpeedValueText;
    [SerializeField] private Text moveSpeedValueText;
    [SerializeField] private Text jumpForceValueText;
    [SerializeField] private GameObject descriptionPanel;
    [SerializeField] private Text descriptionText;

    [Header("Menu")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private int villageSceneNum;

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
        menuPanel.SetActive(false);
    }

    private void Update()
    {
        ShowKillCount();
        ShowStatus();
    }

    public void AdjustingHealthBar(int health, int maxHealth)
    {
        float ratio = (float)health / maxHealth;
        healthBar.fillAmount = ratio;

        healthValue.text = health + " / " + maxHealth;
    }

    public void ContractPanel()
    {
        if (contractPanel.activeInHierarchy == true) CloseContract();
        else OpenContract();
    }

    private void OpenContract()
    {
        contractPanel.SetActive(true);
    }

    private void CloseContract()
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

    private void ShowStatus()
    {
        attackDamageValueText.text = PlayerStatus.instance.Damage.ToString();
        attackSpeedValueText.text = PlayerStatus.instance.AttackSpeed.ToString("0.00");
        moveSpeedValueText.text = PlayerStatus.instance.Speed.ToString();
        jumpForceValueText.text = PlayerStatus.instance.JumpForce.ToString();
    }

    public void ShowDescription(string status)
    {
        string nowText = "Bug...";

        switch (status)
        {
            case "AttackDamage":
                nowText = "마물에게 <color=#225AF6>" + PlayerStatus.instance.Damage.ToString() + "</color>의 피해를 입힐 수 있습니다.";
                break;
            case "AttackSpeed":
                nowText = "1초에 <color=#225AF6>" + PlayerStatus.instance.AttackSpeed.ToString("0.00") + "</color>번 공격할 수 있습니다.";
                break;
            case "MoveSpeed":
                nowText = "1초에 약 <color=#225AF6>" + PlayerStatus.instance.Speed.ToString() + "</color>타일 이동할 수 있습니다.";
                break;
            case "JumpForce":
                nowText = "<color=#225AF6>" + PlayerStatus.instance.JumpForce.ToString() + "</color>의 힘으로 점프합니다.";
                break;
        }

        descriptionPanel.SetActive(true);
        descriptionText.text = nowText;
    }

    public void MoveToTitle()
    {
        SceneManager.LoadScene(0);
    }

    public void Menu()
    {
        if (menuPanel.activeInHierarchy == true)
        {
            menuPanel.SetActive(false);
        }
        else
        {
            menuPanel.SetActive(true);
        }
    }

    public void Menu(bool on)
    {
        menuPanel.SetActive(on);
    }

    public void RestartChapter()
    {
        int villageScene = 2;
        int nowScene = SceneManager.GetActiveScene().buildIndex;
        if (nowScene % 3 == 2) villageScene = nowScene;
        else if (nowScene % 3 == 0) villageScene = nowScene - 1;
        else villageScene = nowScene - 2;

        SceneManager.LoadScene(villageScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
