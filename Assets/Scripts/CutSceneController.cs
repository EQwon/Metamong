using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutSceneController : PrologueController
{
    private Animator animator;

    protected override void Start()
    {
        animator = prologueImagePanel.GetComponent<Animator>();

        base.Start();
    }

    protected override void ShowNowStory()
    {
        base.ShowNowStory();

        if (prologueImagePanel.sprite != null)
        {
            prologueImagePanel.color = Color.white;
            if (animator.enabled)
            {
                for (int i = prologueImagePanel.transform.childCount - 1; i >= 0; i--)
                {
                    Destroy(prologueImagePanel.transform.GetChild(i).gameObject);
                }
                animator.enabled = false;
            }
        }
        else
        {
            prologueImagePanel.color = Color.clear;
            animator.enabled = true;
        }
    }
}