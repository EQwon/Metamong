﻿using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private int health;

    private PlayerMovement mover;
    private PlayerAttack attacker;
    private Animator animator;
    private float horizontalMove = 0;
    private bool jump = false;
    private bool dash = false;
    private bool attack = false;

    private int maxHealth;

    public int MaxHealth { set { maxHealth = value; } }
    public int Health { set { health = value; } }
    public float HealthRatio { get { return (float)health / maxHealth; } }

    private void Awake()
    {
        mover = GetComponent<PlayerMovement>();
        attacker = GetComponent<PlayerAttack>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Health = maxHealth;
    }

    private void Update()
    {
        horizontalMove = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("IsJumping", true);
        }
        if (Input.GetButtonDown("Attack"))
        {
            attack = true;
        }
        if (Input.GetButtonDown("Dash"))
        {
            dash = true;
        }
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    private void FixedUpdate()
    {
        mover.Move(horizontalMove, jump, dash);
        attacker.Attack(horizontalMove, attack);
        jump = false;
        attack = false;
        dash = false;
    }

    public void GetDamage(int amount, GameObject attacker)
    {
        health -= amount;

        Vector2 dir = Vector2.zero;
        dir.x = transform.position.x > attacker.transform.position.x ? 1 : -1;
        dir.y = 0.3f;

        StartCoroutine(mover.KnockBack(dir));
    }
}
