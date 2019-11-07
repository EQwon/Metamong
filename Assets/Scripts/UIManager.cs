using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("Health Bar")]
    [SerializeField] private RectTransform healthBar;
    [SerializeField] private Text healthValue;

    [Header("Info")]
    [SerializeField] private Text killCount;

    [Header("Pause")]
    [SerializeField] private GameObject pausePanel;

    private GameObject player;
    private PlayerInput input;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        input = player.GetComponent<PlayerInput>();
        input.UI = this;

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
