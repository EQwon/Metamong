using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{
    [SerializeField] protected Vector2 exitPos;

    protected GameObject player;

    public virtual void UseGate()
    {
        player.transform.position = exitPos;
    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(exitPos, 0.5f);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            player = coll.gameObject;
            coll.GetComponent<PlayerInput>().NowGate = gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            player = null;
            coll.GetComponent<PlayerInput>().NowGate = null;
        }
    }
}
