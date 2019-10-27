using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateController : MonoBehaviour
{
    [SerializeField] private GameObject guide;

    private float amplitude = 0.1f;
    private Vector2 originPos;

    private void Start()
    {
        originPos = guide.transform.position;
        guide.SetActive(false);
    }

    private void FixedUpdate()
    {
        float nowTime = Time.time;

        guide.transform.position = originPos + new Vector2(0, amplitude * Mathf.Cos(nowTime * 5));
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            guide.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.tag == "Player")
        {
            guide.SetActive(false);
        }
    }
}
