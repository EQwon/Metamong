using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechController : MonoBehaviour
{
    [Header("Resources Holder")]
    [SerializeField] private TextAsset speechAsset;

    [Header("UI Holder")]
    [SerializeField] private GameObject speechPanel;
    [SerializeField] private Text speechText;

    private List<string> speeches;
    private GameObject player;
    private int nowNum = 0;

    private void Start()
    {
        speeches = Parser.SpeechParse(speechAsset);
        speechPanel.SetActive(false);
    }

    public void ShowSpeech()
    {
        speechPanel.SetActive(true);
        ShowNowSpeech();
    }

    public void NextSpeech()
    {
        int targetNum = nowNum + 1;

        if (targetNum >= speeches.Count)
        {
            speechPanel.SetActive(false);
            UIManager.instance.ContractPanel();
            UIManager.instance.CanChangeContract = true;

            nowNum = 0;
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
