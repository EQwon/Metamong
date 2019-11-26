using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarningSignController : MonoBehaviour
{
    [SerializeField] private GameObject bossWarningCanvas;

    private GameObject warningPanel;

    private void Start()
    {
        warningPanel = Instantiate(bossWarningCanvas).transform.GetChild(0).gameObject;
        warningPanel.SetActive(false);
    }

    public void ShowWarningPanel()
    {
        warningPanel.SetActive(true);
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
