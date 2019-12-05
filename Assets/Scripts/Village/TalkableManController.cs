using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkableManController : MonoBehaviour
{
    [SerializeField] private TextAsset speechAsset;
    [SerializeField] private GameObject talkCanavas;
    [SerializeField] private bool shouldShowContract;

    private SpeechController controller;

    private void Start()
    {
        controller = Instantiate(talkCanavas).GetComponent<SpeechController>();
        controller.SpeechAsset = speechAsset;
        controller.ShowContract = shouldShowContract;
        controller.Speaker = gameObject;
    }

    public void StartContract()
    {
        controller.ShowSpeech();
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            coll.gameObject.GetComponent<PlayerInput>().TalkableMan = gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            coll.gameObject.GetComponent<PlayerInput>().TalkableMan = null;
        }
    }
}
