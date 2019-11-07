using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSight : MonoBehaviour
{
    [SerializeField] private bool drawSight = false;
    [SerializeField] [Range(0, 10f)] private float viewRange = 5f;
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private LayerMask obstacleLayer;

    private MonsterAI AI;
    private bool isFacingRight = true;

    private void Start()
    {
        AI = GetComponent<MonsterAI>();
    }

    private void FixedUpdate()
    {
        FindTarget();
    }

    private void OnDrawGizmos()
    {
        if (drawSight == false) return;

        Gizmos.color = Color.grey;

        Gizmos.DrawWireSphere(transform.position, viewRange);
    }

    private void FindTarget()
    {
        Vector2 originPos = transform.position;
        Collider2D[] hittedTargets = Physics2D.OverlapCircleAll(originPos, Mathf.Abs(viewRange), targetLayer);

        if (hittedTargets.Length == 0) return;

        foreach (Collider2D hitTarget in hittedTargets)
        {
            Vector2 targetPos = hitTarget.transform.position;
            Vector2 dir = targetPos - originPos;
            float distance = Mathf.Abs(dir.magnitude);                      // 현재 몬스터와 용사의 거리
            
            if (distance > Mathf.Abs(viewRange)) continue;                  // 거리가 멀면 넘긴다.

            RaycastHit2D rayTarget = Physics2D.Raycast(originPos, dir, Mathf.Abs(viewRange), obstacleLayer);

            // '장애물이 존재하지 않'거나 '장애물이 존재해도 타겟 뒤에 있으면'
            if (!rayTarget || Vector2.Distance(rayTarget.transform.position, originPos) > distance)
            {
                AI.IsFindHero = true;
                AI.Player = hitTarget.gameObject;

                Debug.DrawLine(originPos, targetPos, Color.red);
                enabled = false;
                return;
            }
        }
    }

    public void FlipFacingDir()
    {
        isFacingRight = !isFacingRight;
    }
}
