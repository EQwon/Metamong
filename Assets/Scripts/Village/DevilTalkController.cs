using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DevilTalkController : SpeechController
{
    protected override void EndTalkEvent()
    {
        choicePanel.SetActive(true);
    }

    public void Choose(bool accept)
    {
        int delta = accept ? 1 : 2;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + delta);
    }
}
