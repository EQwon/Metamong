using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterState { Patrol, Chasing, Attack, Hitted, Dead };

public class MonsterAI : MonoBehaviour
{
    [SerializeField] private MonsterState state;
    [SerializeField] protected int health = 100;
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
    private bool freezeState = false;

    private GameObject player;
    
    public bool IsFindHero { set { isFindHero = value; } }
    public bool CanAttackHero { set { canAttackHero = value; } }
    public bool IsHitted { get { return isHitted; } set { isHitted = value; } }
    public bool IsDead { get { return isDead; } set { isDead = value; } }
    public bool FreezeState { set { freezeState = value; } }
    public GameObject Player { get { return player; } set { player = value; } }

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
                mover.Patrol();
                break;
            case MonsterState.Chasing:
                mover.Chasing(player);
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

        if (freezeState) return;

        if (IsHitted)
        {
            state = MonsterState.Hitted;
            return;
        }

        if (isFindHero)
        {
            state = MonsterState.Chasing;

            if (canAttackHero)
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
        if (health <= 0)
        {
            health = 0;
            IsDead = true;
            return;
        }
        healthBarValue.transform.localScale = new Vector3(health / maxHealth, 1, 1);
        IsHitted = true;
        healthBar.SetActive(true);
        StartCoroutine(HitEffect());
    }

    private void Hitted()
    {
        IsHitted = false;
    }

    private IEnumerator HitEffect()
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        sr.color = Color.red;

        yield return new WaitForSeconds(0.05f);

        sr.color = Color.white;
    }

    private void Dead()
    {
        Contract.instance.KillEvent();
        Destroy(gameObject);
    }
}
