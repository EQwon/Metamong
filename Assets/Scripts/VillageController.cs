using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VillageController : MonoBehaviour
{
    [Header("Resources Holder")]
    [SerializeField] private List<string> speeches;

    [Header("UI Holder")]
    [SerializeField] private GameObject contractPanel;
    [SerializeField] private GameObject speechPanel;
    [SerializeField] private Text speechText;

    private GameObject player;
    private int nowNum = 0;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerInput>().enabled = false;
        speechPanel.SetActive(true);
        contractPanel.SetActive(false);

        ShowNowSpeech();
    }

    public void NextSpeech()
    {
        int targetNum = nowNum + 1;

        if (targetNum >= speeches.Count)
        {
            speechPanel.SetActive(false);
            contractPanel.SetActive(true);
            return;
        }

        nowNum = targetNum;
        ShowNowSpeech();
    }

    private void ShowNowSpeech()
    {
        speechText.text = speeches[nowNum];
    }
}
