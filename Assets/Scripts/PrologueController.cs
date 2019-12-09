using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PrologueController : MonoBehaviour
{
    [Header("Resources Holder")]
    [SerializeField] private List<Sprite> prologueImages;
    [SerializeField] private List<string> prologueSpeeches;

    [Header("UI Holder")]
    [SerializeField] protected Image prologueImagePanel;
    [SerializeField] protected Text prologueText;

    private int nowNum = 0;

    protected virtual void Start()
    {
        ShowNowStory();
    }

    public void NextPrologue()
    {
        int targetNum = nowNum + 1;

        if (targetNum >= prologueImages.Count)
        {
            EndEvent();
            return;
        }

        nowNum = targetNum;
        ShowNowStory();
    }

    protected virtual void ShowNowStory()
    {
        prologueImagePanel.sprite = prologueImages[nowNum];
        prologueText.text = prologueSpeeches[nowNum];
    }

    protected virtual void EndEvent()
    {
        SceneManager.LoadScene(2);
    }
}