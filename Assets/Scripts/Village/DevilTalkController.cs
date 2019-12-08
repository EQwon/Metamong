using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevilTalkController : SpeechController
{
    protected override void EndTalkEvent()
    {
        choicePanel.SetActive(true);
    }

    public void Choose(bool accept)
    {

    }
}
