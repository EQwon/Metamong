using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MonsterMovement : MonoBehaviour
{
    [SerializeField] private float patrolStartX;
    [SerializeField] private float patrolEndX;
    [SerializeField] private float speed = 3f;

    private Rigidbody2D m_Rigidbody2D;
    private Vector3 m_Velocity = Vector3.zero;
    private bool m_FacingRight = true;
    const float m_MovementSmoothing = 0.05f;
    private Vector2 targetVelocity;

    private void Awake()
    {
        patrolStartX += transform.position.x;
        patrolEndX += transform.position.x;

        m_Rigidbody2D = GetComponent<Rigidbody2D>();
        targetVelocity = new Vector2(speed, m_Rigidbody2D.velocity.y);
    }

    private void FixedUpdate()
    {
        Patrol();
    }

    public void Patrol()
    {
        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, m_MovementSmoothing);

        if (transform.position.x >= patrolEndX && m_FacingRight)
        {
            Flip();
        }
        else if (transform.position.x <= patrolStartX && !m_FacingRight)
        {
            Flip();
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

    private void Flip()
    {
        m_FacingRight = !m_FacingRight;
        GetComponent<MonsterSight>().FlipFacingDir();
        GetComponent<MonsterBasicAttack>().FlipFacingDir();

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        targetVelocity *= Vector2.left;
    }
}
