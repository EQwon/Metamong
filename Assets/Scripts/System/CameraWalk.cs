﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWalk : MonoBehaviour
{
    [SerializeField] private bool drawCameraArea = false;
    [SerializeField] private float leftEdge;
    [SerializeField] private float rightEdge;
    [SerializeField] private Vector2 upperLimit;
    [SerializeField] private Vector2 lowerLimit;
    [SerializeField] private float cameraSpeed;

    private Vector3 targetPos;
    private GameObject player;

    private Vector2 UpperLimit { get { return (Vector2)transform.position + upperLimit; } }
    private Vector2 LowerLimit { get { return (Vector2)transform.position + lowerLimit; } }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").gameObject;
        
        targetPos = (Vector2)player.transform.position;
        targetPos.z = -10f;
    }

    private void FixedUpdate()
    {
        float playerPosX = player.transform.position.x;
        float playerPosY = player.transform.position.y;

        // 카메라 존
        if (playerPosX > UpperLimit.x) targetPos.x = playerPosX - upperLimit.x;
        else if (playerPosX < LowerLimit.x) targetPos.x = playerPosX - lowerLimit.x;

        if (playerPosY > UpperLimit.y) targetPos.y = playerPosY - upperLimit.y;
        else if (playerPosY < LowerLimit.y) targetPos.y = playerPosY - lowerLimit.y;

        // 엣지
        float cameraSize = 2 * Camera.main.orthographicSize;
        if (targetPos.x < leftEdge + cameraSize) targetPos.x = leftEdge + cameraSize;
        if (targetPos.x > rightEdge - cameraSize) targetPos.x = rightEdge - cameraSize;

        if (Input.GetAxisRaw("Vertical") > 0)
        {
            targetPos.y = transform.position.y + cameraSpeed * Time.fixedDeltaTime;
            if (targetPos.y > playerPosY + upperLimit.y)
                targetPos.y = playerPosY + upperLimit.y;
        }
        if (Input.GetAxisRaw("Vertical") < 0)
        {
            targetPos.y = transform.position.y - cameraSpeed * Time.fixedDeltaTime;
            if (targetPos.y < playerPosY + lowerLimit.y)
                targetPos.y = playerPosY + lowerLimit.y;
        }

        transform.position = targetPos;
    }

    private void OnDrawGizmos()
    {
        if (!drawCameraArea) return;

        Gizmos.color = Color.blue;

        Gizmos.DrawRay(new Vector2(leftEdge, 0), 500 * Vector2.up);
        Gizmos.DrawRay(new Vector2(leftEdge, 0), 500 * Vector2.down);
        Gizmos.DrawRay(new Vector2(rightEdge, 0), 500 * Vector2.up);
        Gizmos.DrawRay(new Vector2(rightEdge, 0), 500 * Vector2.down);

        Gizmos.DrawLine(LowerLimit, new Vector2(LowerLimit.x, UpperLimit.y));
        Gizmos.DrawLine(LowerLimit, new Vector2(UpperLimit.x, LowerLimit.y));
        Gizmos.DrawLine(UpperLimit, new Vector2(LowerLimit.x, UpperLimit.y));
        Gizmos.DrawLine(UpperLimit, new Vector2(UpperLimit.x, LowerLimit.y));
    }

    public IEnumerator Shaking()
    {
        int cnt = Random.Range(5, 10);

        for (int i = 0; i < cnt; i++)
        {
            Vector3 randomShake = new Vector2(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f));

            transform.position = transform.position + randomShake;

            yield return new WaitForFixedUpdate();
        }
    }
}
