using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MonsterMovement : MonoBehaviour
{
    [Header("Patrol")]
    [Tooltip("Patrol 왼쪽 끝")]
    [SerializeField] private float patrolLeftEnd;
    [Tooltip("Patrol 오른쪽 끝")]
    [SerializeField] private float patrolRightEnd;
    [Tooltip("Patrol 속도")]
    [SerializeField] private float speed = 4f;
    [Tooltip("Patrol 방향 전환 대기 시간")]
    [SerializeField] [Range(0, 3f)] private float waitingTime = 0.5f;

    [Header("Chasing")]
    [Tooltip("Chasing 속도")]
    [SerializeField] private float chasingSpeed = 6f;

    [Header("KnockBack")]
    [Tooltip("플레이어의 공격에 의한 넉백의 정도")]
    [SerializeField] [Range(0, 1000f)] private float knockBackForce = 400f;
    [Tooltip("플레이어의 공격에 의한 경직 시간")]
    [SerializeField] [Range(0, 3f)] private float knockBackFreezeTime = 1f;

    private Animator animator;
    private Rigidbody2D m_Rigidbody2D;
    private Vector3 myVelocity = Vector3.zero;
    private bool isFacingRight = true;
    private float movementSmoothing = 0.05f;
    private Vector2 targetVelocity;
    private float damp = 0.6f;
    private bool isWaiting = false;
    private bool isFreeze = false;

    private void Awake()
    {
        PatrolAreaSwap();
        patrolLeftEnd += transform.position.x;
        patrolRightEnd += transform.position.x;

        animator = GetComponent<Animator>();
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        targetVelocity = new Vector2(speed, m_Rigidbody2D.velocity.y);
    }

    private void FixedUpdate()
    {
        targetVelocity.y = m_Rigidbody2D.velocity.y;
        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref myVelocity, movementSmoothing);
        animator.SetFloat("Speed", Mathf.Abs(m_Rigidbody2D.velocity.x));
    }

    public void Patrol()
    {
        if (isFreeze) return;

        // 오른쪽 끝에 도달했을 경우
        if (transform.position.x >= patrolRightEnd && isFacingRight)
        {
            targetVelocity.x = 0;
            StartCoroutine(Waiting());
        }
        // 왼쪽 끝에 도달했을 경우
        else if (transform.position.x <= patrolLeftEnd && !isFacingRight)
        {
            targetVelocity.x = 0;
            StartCoroutine(Waiting());
        }
        // patrol 범위 사이일 경우
        else
        {
            targetVelocity.x = isFacingRight ? speed : -speed;
        }
    }

    public void Chasing(Vector2 targetPos)
    {
        if (isFreeze) return;

        if (transform.position.x < targetPos.x && !isFacingRight) Flip();
        if (targetPos.x < transform.position.x && isFacingRight) Flip();

        if (transform.position.x >= patrolRightEnd && isFacingRight)
        {
            targetVelocity.x = 0;
        }
        else if (transform.position.x <= patrolLeftEnd && !isFacingRight)
        {
            targetVelocity.x = 0;
        }
        else
        {
            targetVelocity.x = isFacingRight ? chasingSpeed : -chasingSpeed;
        }
    }

    public void Attack(Vector2 targetPos)
    {
        if (transform.position.x < targetPos.x && !isFacingRight) Flip();
        if (targetPos.x < transform.position.x && isFacingRight) Flip();
    }

    public void Rush(float rushSpeed)
    {
        if (transform.position.x >= patrolRightEnd && isFacingRight)
        {
            targetVelocity.x = 0;
        }
        else if (transform.position.x <= patrolLeftEnd && !isFacingRight)
        {
            targetVelocity.x = 0;
        }
        else
        {
            targetVelocity.x = isFacingRight ? rushSpeed : -rushSpeed;
        }
    }

    private void OnDrawGizmos()
    {
        #if UNITY_EDITOR
        if (!EditorApplication.isPlaying)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position + new Vector3(patrolLeftEnd, 0.1f), transform.position + new Vector3(patrolRightEnd, 0.1f));
        }
        else
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(new Vector2(patrolLeftEnd, transform.position.y), new Vector2(patrolRightEnd, transform.position.y));
        }
        #endif
    }

    private IEnumerator Waiting()
    {
        if (isWaiting) yield break;

        isWaiting = true;

        yield return new WaitForSeconds(waitingTime);

        isWaiting = false;

        Flip();
    }

    public IEnumerator KnockBack(GameObject player)
    {
        isFreeze = true;
        m_Rigidbody2D.velocity = Vector2.zero;

        float horizontalForce = player.transform.position.x > transform.position.x ? -knockBackForce : knockBackForce;
        m_Rigidbody2D.AddForce(new Vector2(horizontalForce, 100f));

        yield return new WaitForSeconds(knockBackFreezeTime);

        isFreeze = false;
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        GetComponent<MonsterSight>().FlipFacingDir();
        GetComponent<MonsterAttack>().FlipFacingDir();

        GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
    }

    private void PatrolAreaSwap()
    {
        // patrolLeftEnd 가 왼쪽 끝, patrolRightEnd 가 오른쪽 끝임을 보장.
        if (patrolLeftEnd > patrolRightEnd)
        {
            float temp = patrolRightEnd;
            patrolRightEnd = patrolLeftEnd;
            patrolLeftEnd = temp;
        }
    }
}
