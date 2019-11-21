﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int damage = 5;
    [SerializeField] private float attackPostDelay = 1f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float movementDamping = 0.05f;
    [SerializeField] private float jumpForce = 700f;
    [SerializeField] private float invincibleTime;
    [SerializeField] private float knockBackForce;

    public int MaxHealth { set { maxHealth = value; } }
    public int Damage { get { return damage; } set { damage = value; } }
    public float AttackPostDelay { set { attackPostDelay = value; } }
    public float AttackSpeed { get { return 1 / (0.1f + attackPostDelay); } }
    public float Speed { get { return speed; } set { speed = value; } }
    public float MovementDamping { set { movementDamping = value; } }
    public float JumpForce { get { return jumpForce; } set { jumpForce = value; } }
    public float InvincibleTime { set { invincibleTime = value; } }
    public float KnockBackForce { set { knockBackForce = value; } }

    private PlayerInput input;
    private PlayerMovement mover;
    private PlayerAttack attacker;

    public static PlayerStatus instance;

    private void Awake()
    {
        if (instance != this) instance = this;

        input = GetComponent<PlayerInput>();
        mover = GetComponent<PlayerMovement>();
        attacker = GetComponent<PlayerAttack>();

        UpdateStatus();
    }

    private void Start()
    {
        Contract.instance.KillContractCheck();
    }

    public void UpdateStatus()
    {
        Debug.Log("스탯 업데이트");
        input.MaxHealth = maxHealth;
        attacker.Damage = damage;
        attacker.AttackPostDelay = attackPostDelay;
        mover.Speed = speed;
        mover.MovementDamping = movementDamping;
        mover.JumpForce = jumpForce;
        input.InvincibleTime = invincibleTime;
        mover.KnockBackForce = knockBackForce;
    }
}
