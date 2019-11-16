using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningSignController : MonoBehaviour
{
    [SerializeField] private GameObject bossWarningPanel;

    private void Start()
    {
        bossWarningPanel.SetActive(false);
    }

    public void ShowWarningPanel()
    {
        bossWarningPanel.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            coll.gameObject.GetComponent<PlayerInput>().WarningSign = gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            coll.gameObject.GetComponent<PlayerInput>().WarningSign = null;
        }
    }
}
