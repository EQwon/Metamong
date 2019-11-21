using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GateController : MonoBehaviour
{
    [SerializeField] private int sceneNum;
    [SerializeField] private Vector2 startPos;

    public int SceneNum { set { sceneNum = value; } }

    public void EnterGate()
    {
        PlayerStatus.instance.AdjustStartPos(startPos);
        SceneManager.LoadScene(sceneNum);
    }
    
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            coll.GetComponent<PlayerInput>().NowGate = gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            coll.GetComponent<PlayerInput>().NowGate = null;
        }
    }
}
