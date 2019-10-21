using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraWalk : MonoBehaviour
{
    [SerializeField] private bool drawCameraArea = false;
    [SerializeField] private float leftEdge;
    [SerializeField] private float rightEdge;
    [SerializeField] private Vector2 upperLimit;
    [SerializeField] private Vector2 lowerLimit;

    private Vector3 targetPos;
    private GameObject player;

    private Vector2 UpperLimit { get { return (Vector2)transform.position + upperLimit; } }
    private Vector2 LowerLimit { get { return (Vector2)transform.position + lowerLimit; } }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").gameObject;
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
}
