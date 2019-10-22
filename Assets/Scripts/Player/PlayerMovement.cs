using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] [Range(500, 1500f)] private float knockBackForce = 600f;
    [SerializeField] [Range(0, 3f)] private float knockBackFreezeTime = 0.5f;
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private LayerMask m_WhatIsGround;

    private float speed;
    private float m_JumpForce;

    private Vector2 groundChecker { get { return new Vector2(0, -1.1f) + (Vector2)transform.position; } }
    const float k_GroundedRadius = 0.2f;
    private bool m_Grounded;
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;
    private Vector3 m_Velocity = Vector3.zero;
    private float movementDamping = 0.05f;

    public float Speed { set { speed = value; } }
    public float JumpForce { set { m_JumpForce = value; } }
    public float MovementDamping { set { movementDamping = value; } }

    [Header("Events")]
    public UnityEvent OnLandEvent;

    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    }

    private void FixedUpdate()
    {
        bool wasGrounded = m_Grounded;
        m_Grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundChecker, k_GroundedRadius, m_WhatIsGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                m_Grounded = true;
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }
        }
    }

    public void Move(float move, bool jump)
    {
        Vector3 targetVelocity = new Vector2(move * speed, m_Rigidbody2D.velocity.y);
        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, movementDamping);

        if (move > 0 && !m_FacingRight)
        {
            Flip();
        }
        else if (move < 0 && m_FacingRight)
        {
            Flip();
        }

        if (m_Grounded && jump)
        {
            m_Rigidbody2D.AddForce(new Vector2(0f, m_JumpForce));
        }
    }

    public void Dash()
    {
        Vector3 targetVelocity = m_Rigidbody2D.velocity;
        targetVelocity.x = m_FacingRight ? dashSpeed : -dashSpeed;

        m_Rigidbody2D.velocity = targetVelocity;
    }

    private void Flip()
    {
        m_FacingRight = !m_FacingRight;
        
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public IEnumerator KnockBack(Vector2 dir)
    {
        m_Rigidbody2D.velocity = Vector2.zero;

        Vector2 force = dir.normalized * knockBackForce;
        m_Rigidbody2D.AddForce(force);

        yield return new WaitForSeconds(knockBackFreezeTime);
    }
}