using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] [Range(500, 1500f)] private float knockBackForce = 600f;
    [SerializeField] [Range(0, 3f)] private float knockBackFreezeTime = 0.5f;
    [SerializeField] private float dashSpeed = 1000f;
    [SerializeField] private LayerMask m_WhatIsGround;

    private float speed;
    private float m_JumpForce;

    private Vector2 groundChecker { get { return new Vector2(0, -0.4f) + (Vector2)transform.position; } }
    const float k_GroundedRadius = 0.2f;
    private bool m_Grounded;
    private bool isDashing = false;
    private Rigidbody2D m_Rigidbody2D;
    private bool m_FacingRight = true;
    private Vector3 m_Velocity = Vector3.zero;
    private float movementDamping = 0.05f;

    private Vector3 targetVelocity = Vector3.zero;

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

        if (isDashing)
            m_Rigidbody2D.velocity = targetVelocity;
        else
            m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, targetVelocity, ref m_Velocity, movementDamping);
    }

    public void Move(float move, bool jump, bool dash)
    {
        if (isDashing) return;

        if (dash)
        {
            targetVelocity.x = m_FacingRight ? dashSpeed : -dashSpeed;
            StartCoroutine(Dashing());
            return;
        }

        targetVelocity = new Vector2(move * speed, m_Rigidbody2D.velocity.y);

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

    private IEnumerator Dashing()
    {
        isDashing = true;

        yield return new WaitForSeconds(0.2f);

        isDashing = false;
    }
}