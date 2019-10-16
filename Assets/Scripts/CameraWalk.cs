using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWalk : MonoBehaviour
{
    [SerializeField] private float leftHorizontalLimit;
    [SerializeField] private float rightHorizontalLimit;

    private Vector3 targetPos;
    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").gameObject;
        targetPos.z = -10f;
    }

    private void FixedUpdate()
    {
        targetPos.x = player.transform.position.x;
        targetPos.y = player.transform.position.y;

        if (targetPos.x < leftHorizontalLimit)
            targetPos.x = leftHorizontalLimit;

        if (targetPos.x > rightHorizontalLimit)
            targetPos.x = rightHorizontalLimit;

        transform.position = targetPos;
    }
}
