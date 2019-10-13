using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MonsterMovement : MonoBehaviour
{
    [SerializeField] private float patrolStartX;
    [SerializeField] private float patrolEndX;
    [SerializeField] private float speed = 4f;
    [SerializeField] private float chasingSpeed = 6f;
    [SerializeField] private float waitingTime = 0.5f;

    private Rigidbody2D m_Rigidbody2D;
    private Vector3 m_Velocity = Vector3.zero;
    private bool m_FacingRight = true;
    const float m_MovementSmoothing = 0.05f;
    private Vector2 targetVelocity;
    private float damp = 0.2f;
    private bool isWaiting = false;

    private void Awake()
    {
        patrolStartX += transform.position.x;
        patrolEndX += transform.position.x;

        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        targetVelocity = new Vector2(speed, m_Rigidbody2D.velocity.y);
    }

    private void FixedUpdate()
    {
        Boarder();
    }

    private void Boarder()
    {
        if (transform.position.x > patrolEndX + damp) m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
        if (transform.position.x < patrolStartX - damp) m_Rigidbody2D.velocity = new Vector2(0, m_Rigidbody2D.velocity.y);
    }

    // 오른쪽 끝 라인에 다달았을 때 정지

    // 0.5초 후 왼쪽으로 이동
    // 왼쪽 끝 라인에 다달았을 때 정지
    // 0.5 초 후 오른쪽으로 이동
    public void Patrol()
    {
        if (transform.position.x >= patrolEndX && m_FacingRight)
        {
            targetVelocity = Vector2.zero;
            StartCoroutine(Waiting());
        }
        else if (transform.position.x <= patrolStartX && !m_FacingRight)
        {
            targetVelocity = Vector2.zero;
            StartCoroutine(Waiting());
        }
        else
        {
            targetVelocity = m_FacingRight ? new Vector2(speed, m_Rigidbody2D.velocity.y) : new Vector2(-speed, m_Rigidbody2D.velocity.y);

            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);
        }
    }

    public void Chasing()
    {
        
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

    private void Flip()
    {
        m_FacingRight = !m_FacingRight;
        GetComponent<MonsterSight>().FlipFacingDir();
        GetComponent<MonsterBasicAttack>().FlipFacingDir();

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
