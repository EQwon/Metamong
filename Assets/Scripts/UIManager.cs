using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private RectTransform healthBar;
    [SerializeField] private GameObject pausePanel;

    private GameObject player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerInput>().UI = this;

        pausePanel.SetActive(false);
    }

    private void Update()
    {
        Vector3 targetScale = Vector3.one;
        targetScale.x = player.GetComponent<PlayerInput>().HealthRatio;
        healthBar.localScale = targetScale;
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
