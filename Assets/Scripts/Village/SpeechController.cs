using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeechController : MonoBehaviour
{
    [Header("UI Holder")]
    [SerializeField] private GameObject speechPanel;
    [SerializeField] protected GameObject choicePanel;
    [SerializeField] private Text speechText;

    private GameObject speaker;
    private bool showContract;
    private TextAsset speechAsset;
    private List<List<string>> speeches;
    private GameObject player;
    private int talkCnt = 0;
    private int nowNum = 0;

    public TextAsset SpeechAsset { set { speechAsset = value; } }
    public GameObject Speaker { set { speaker = value; } }
    public bool ShowContract { set { showContract = value; } }

    private void Start()
    {
        speeches = Parser.SpeechParse(speechAsset);
        speechPanel.SetActive(false);
        choicePanel.SetActive(false);
    }

    public void ShowSpeech()
    {
        speechPanel.SetActive(true);
        ShowNowSpeech();
    }

    public void NextSpeech()
    {
        int targetNum = nowNum + 1;

        if (targetNum >= speeches[talkCnt].Count)
        {
            speechPanel.SetActive(false);
            EndTalkEvent();

            nowNum = 0;
            return;
        }

        nowNum = targetNum;
        ShowNowSpeech();
    }

    private void ShowNowSpeech()
    {
        speechText.text = speeches[talkCnt][nowNum];
    }

    protected virtual void EndTalkEvent()
    {
        if (showContract)
        {
            UIManager.instance.ContractPanel();
            UIManager.instance.CanChangeContract = true;
        }

        talkCnt += 1;
        if (speeches.Count <= talkCnt) talkCnt -= 1;
    }

    private IEnumerator Die()
    {
        speaker.GetComponent<Collider2D>().enabled = false;

        speaker.GetComponent<Animator>().enabled = true;
        yield return new WaitForSeconds(1f);

        Destroy(speaker);
    }
}
