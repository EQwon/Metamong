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
    [SerializeField] private Image prologueImagePanel;
    [SerializeField] private Text prologueText;

    private int nowNum = 0;

    private void Start()
    {
        ShowNowPrologue();
    }

    public void NextPrologue()
    {
        int targetNum = nowNum + 1;

        if (targetNum >= prologueImages.Count)
        {
            SceneManager.LoadScene(2);
            return;
        }

        nowNum = targetNum;
        ShowNowPrologue();
    }

    private void ShowNowPrologue()
    {
        prologueImagePanel.sprite = prologueImages[nowNum];
        prologueText.text = prologueSpeeches[nowNum];
    }
}