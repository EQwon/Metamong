using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OldManController : MonoBehaviour
{
    [SerializeField] private GameObject talkCanavas;

    private SpeechController controller;

    private void Start()
    {
        controller = Instantiate(talkCanavas).GetComponent<SpeechController>();
    }

    public void StartContract()
    {
        controller.ShowSpeech();
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            coll.gameObject.GetComponent<PlayerInput>().OldMan = gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            coll.gameObject.GetComponent<PlayerInput>().OldMan = null;
        }
    }
}
