using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterState { Patrol, Chasing, Attack, Hitted, Dead };

public class MonsterAI : MonoBehaviour
{
    [SerializeField] private MonsterState state;
    [SerializeField] private int health = 100;
    [SerializeField] private bool isFindHero = false;
    [SerializeField] private bool canAttackHero = false;
    [SerializeField] private bool isHitted = false;
    [SerializeField] private bool isDead = false;

    private MonsterMovement mover;
    private MonsterSight sight;
    private MonsterAttack attacker;
    private GameObject healthBar;
    private GameObject healthBarValue;
    private float maxHealth;

    private Vector2 playerPos = Vector2.zero;

    public bool IsFindHero { get { return isFindHero; } set { isFindHero = value; } }
    public bool CanAttackHero { get { return canAttackHero; } set { canAttackHero = value; } }
    public bool IsHitted { get { return isHitted; } set { isHitted = value; } }
    public bool IsDead { get { return isDead; } set { isDead = value; } }
    public Vector2 PlayerPos { get { return playerPos; } set { playerPos = value; } }

    private void Awake()
    {
        mover = GetComponent<MonsterMovement>();
        sight = GetComponent<MonsterSight>();
        attacker = GetComponent<MonsterAttack>();

        maxHealth = health;
        healthBar = transform.GetChild(0).gameObject;
        healthBarValue = healthBar.transform.GetChild(0).gameObject;
        healthBar.SetActive(false);
    }

    private void FixedUpdate()
    {
        SwitchState();

        switch (state)
        {
            case MonsterState.Patrol:
                GetComponent<SpriteRenderer>().color = Color.white;
                mover.Patrol();
                break;
            case MonsterState.Chasing:
                GetComponent<SpriteRenderer>().color = Color.yellow;
                mover.Chasing();
                break;
            case MonsterState.Attack:
                GetComponent<SpriteRenderer>().color = Color.red;
                mover.Attack();
                attacker.TryAttack();
                break;
            case MonsterState.Hitted:
                GetComponent<SpriteRenderer>().color = Color.grey;
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
                return;
            }
            return;
        }

        state = MonsterState.Patrol;
    }

    public void GetDamage(int amount, GameObject player)
    {
        health -= amount;
        healthBarValue.transform.localScale = new Vector3(health / maxHealth, 1, 1);
        IsHitted = true;
        StartCoroutine(mover.KnockBack(player));

        if (health <= 0) IsDead = true;
    }

    private void Hitted()
    {
        healthBar.SetActive(true);
        IsHitted = false;
    }

    private void Dead()
    {
        Contract.instance.KillEvent();
        Destroy(gameObject);
    }
}
