using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterState { Patrol, Chasing, Attack, Hitted, Dead };

public class MonsterAI : MonoBehaviour
{
    [SerializeField] private MonsterState state;
    [SerializeField] private int health = 10;
    [SerializeField] private bool isFindHero = false;
    [SerializeField] private bool canAttackHero = false;
    [SerializeField] private bool isHitted = false;
    [SerializeField] private bool isDead = false;

    private MonsterMovement mover;
    private MonsterSight sight;
    private MonsterBasicAttack attacker;

    public bool IsFindHero { get { return isFindHero; } set { isFindHero = value; } }
    public bool CanAttackHero { get { return canAttackHero; } set { canAttackHero = value; } }
    public bool IsHitted { get { return isHitted; } set { isHitted = value; } }
    public bool IsDead { get { return isDead; } set { isDead = value; } }

    private void Awake()
    {
        mover = GetComponent<MonsterMovement>();
        sight = GetComponent<MonsterSight>();
        attacker = GetComponent<MonsterBasicAttack>();
    }

    private void FixedUpdate()
    {
        SwitchState();

        switch (state)
        {
            case MonsterState.Patrol:
                mover.Patrol();
                break;
            case MonsterState.Chasing:
                mover.Chasing();
                break;
            case MonsterState.Attack:
                attacker.TryAttack();
                break;
            case MonsterState.Hitted:
                Hitted();
                break;
            case MonsterState.Dead:
                Dead();
                break;
        }
    }

    private void SwitchState()
    {
        if (IsDead)
        {
            state = MonsterState.Dead;
            return;
        }

        if (IsHitted)
        {
            state = MonsterState.Hitted;
            return;
        }

        if (IsFindHero)
        {
            state = MonsterState.Chasing;

            if (CanAttackHero)
            {
                state = MonsterState.Attack;
                CanAttackHero = false;
                return;
            }
            return;
        }

        state = MonsterState.Patrol;
    }

    public void GetDamage(int amount)
    {
        health -= amount;
        IsHitted = true;

        if (health <= 0) IsDead = true;
    }

    private void Hitted()
    {
        IsHitted = false;
    }

    private void Dead()
    {
        Destroy(gameObject);
    }
}
