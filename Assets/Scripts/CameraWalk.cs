using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWalk : MonoBehaviour
{
    [SerializeField] private float leftLimit;
    [SerializeField] private float rightLimit;

    private Vector3 targetPos;
    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").gameObject;
        targetPos.y = transform.position.y;
        targetPos.z = -10f;
    }

    private void FixedUpdate()
    {
        targetPos.x = player.transform.position.x;

        if (targetPos.x < leftLimit)
            targetPos.x = leftLimit;

        if (targetPos.x > rightLimit)
            targetPos.x = rightLimit;

        transform.position = targetPos;
    }
}
