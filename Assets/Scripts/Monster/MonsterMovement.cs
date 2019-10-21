using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MonsterMovement : MonoBehaviour
{
    [Header("Patrol")]
    [Tooltip("Patrol 왼쪽 끝")]
    [SerializeField] private float patrolStartX;
    [Tooltip("Patrol 오른쪽 끝")]
    [SerializeField] private float patrolEndX;
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

    private Rigidbody2D m_Rigidbody2D;
    private Vector3 m_Velocity = Vector3.zero;
    private bool m_FacingRight = true;
    const float m_MovementSmoothing = 0.05f;
    private Vector2 targetVelocity;
    private float damp = 0.6f;
    private bool isWaiting = false;
    private bool isFreeze = false;

    private void Awake()
    {
        patrolStartX += transform.position.x;
        patrolEndX += transform.position.x;

        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        targetVelocity = new Vector2(speed, m_Rigidbody2D.velocity.y);
    }

    private void FixedUpdate()
    {
        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

        Boarder();
    }

    private void Boarder()
    {
        if (transform.position.x > patrolEndX + damp) m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
        if (transform.position.x < patrolStartX - damp) m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
    }

    public void Patrol()
    {
        if (isFreeze) return;

        // 오른쪽 끝에 도달했을 경우
        if (transform.position.x >= patrolEndX && m_FacingRight)
        {
            targetVelocity = Vector2.zero;
            StartCoroutine(Waiting());
        }
        // 왼쪽 끝에 도달했을 경우
        else if (transform.position.x <= patrolStartX && !m_FacingRight)
        {
            targetVelocity = Vector2.zero;
            StartCoroutine(Waiting());
        }
        // patrol 범위 사이일 경우
        else
        {
            targetVelocity.y = m_Rigidbody2D.velocity.y;
            targetVelocity.x = m_FacingRight ? speed : -speed;
        }
    }

    public void Chasing(Vector2 targetPos)
    {
        if (isFreeze) return;

        if (transform.position.x < targetPos.x && !m_FacingRight) Flip();
        if (targetPos.x < transform.position.x && m_FacingRight) Flip();

        if (transform.position.x >= patrolEndX && m_FacingRight)
        {
            targetVelocity = Vector2.zero;
        }
        else if (transform.position.x <= patrolStartX && !m_FacingRight)
        {
            targetVelocity = Vector2.zero;
        }
        else
        {
            targetVelocity.y = m_Rigidbody2D.velocity.y;
            targetVelocity.x = m_FacingRight ? chasingSpeed : -chasingSpeed;
        }
    }

    public void Attack(Vector2 targetPos)
    {
        if (transform.position.x < targetPos.x && !m_FacingRight) Flip();
        if (targetPos.x < transform.position.x && m_FacingRight) Flip();

        targetVelocity = Vector2.zero;
    }

    public void Rush(float rushSpeed)
    {
        if (transform.position.x >= patrolEndX && m_FacingRight)
        {
            targetVelocity = Vector2.zero;
        }
        else if (transform.position.x <= patrolStartX && !m_FacingRight)
        {
            targetVelocity = Vector2.zero;
        }
        else
        {
            targetVelocity.y = m_Rigidbody2D.velocity.y;
            targetVelocity.x = m_FacingRight ? rushSpeed : -rushSpeed;
        }
    }

    private void OnDrawGizmos()
    {
        if (!EditorApplication.isPlaying)
        {
            Gizmos.color = Color.grey;
            Gizmos.DrawLine(transform.position + new Vector3(patrolStartX, 0), transform.position + new Vector3(patrolEndX, 0));
        }
        else
        {
            Gizmos.color = Color.grey;
            Gizmos.DrawLine(new Vector2(patrolStartX, transform.position.y), new Vector2(patrolEndX, transform.position.y));
        }
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
        m_FacingRight = !m_FacingRight;
        GetComponent<MonsterSight>().FlipFacingDir();
        GetComponent<MonsterAttack>().FlipFacingDir();

        GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
    }
}
