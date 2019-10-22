using System.Collections;
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

    public int MaxHealth { set { maxHealth = value; } }
    public int Damage { set { damage = value; } }
    public float AttackPostDelay { set { attackPostDelay = value; } }
    public float Speed { set { speed = value; } }
    public float MovementDamping { set { movementDamping = value; } }
    public float JumpForce { set { jumpForce = value; } }

    private PlayerInput health;
    private PlayerMovement mover;
    private PlayerAttack attacker;

    private void Awake()
    {
        health = GetComponent<PlayerInput>();
        mover = GetComponent<PlayerMovement>();
        attacker = GetComponent<PlayerAttack>();

        UpdateStatus();
    }

    public void UpdateStatus()
    {
        health.Health = maxHealth;
        mover.Speed = speed;
        mover.MovementDamping = movementDamping;
        mover.JumpForce = jumpForce;
        attacker.Damage = damage;
        attacker.AttackPostDelay = attackPostDelay;
    }
}
