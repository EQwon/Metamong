using System.Collections;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private WeaponDelay delay;
    [SerializeField] private int attackDamage = 5;
    [SerializeField] [Range(0, 10f)] private float attackRange;
    [SerializeField] private LayerMask attackLayer;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private Vector2 attackAreaPos;
    [SerializeField] private Vector2 attackArea;

    protected MonsterAI AI;
    protected MonsterMovement mover;
    private Animator animator;
    private int isFacingRight = 1;
    private bool canAttack = true;
    private bool doingAttack = false;

    private Vector2 attackPos { get { return new Vector2(isFacingRight * attackAreaPos.x, attackAreaPos.y) + (Vector2)transform.position; } }
    private Vector2 attackSize { get { return new Vector2(attackArea.x, attackArea.y); } }
    public int IsFacingRight { get { return isFacingRight; } }
    public int AttackDamage { get { return attackDamage; } }
    public LayerMask AttackLayer { get { return attackLayer; } }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        AI = GetComponent<MonsterAI>();
        mover = GetComponent<MonsterMovement>();
    }

    private void FixedUpdate()
    {
        Approach();

        if (doingAttack)
        {
            DoAttack();
        }
    }

    public virtual void DoAttack()
    {
        Collider2D collider = Physics2D.OverlapBox(attackPos, attackSize, 0, attackLayer);
        if (collider) ApplyDamage(collider);
    }

    public void TryAttack()
    {
        if (canAttack)
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        if (canAttack == false) yield break;

        canAttack = false;
        animator.SetBool("Attack", true);
        mover.StopForAttack();

        yield return new WaitForSeconds(delay.pre);

        doingAttack = true;

        yield return new WaitForSeconds(delay.attack);

        doingAttack = false;
        animator.SetBool("Attack", false);

        yield return new WaitForSeconds(delay.post);

        canAttack = true;
        AI.CanAttackHero = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPos, attackSize);
    }

    private void ApplyDamage(Collider2D coll)
    {
        GameObject hero = coll.gameObject;
        hero.GetComponent<PlayerInput>().GetDamage(attackDamage, gameObject);
    }

    public void FlipFacingDir()
    {
        isFacingRight *= -1;
    }

    private void Approach()
    {
        Vector2 originPos = transform.position;
        Collider2D[] hittedTargets = Physics2D.OverlapCircleAll(originPos, Mathf.Abs(attackRange), attackLayer);

        if (hittedTargets.Length == 0) return;

        foreach (Collider2D hitTarget in hittedTargets)
        {
            Vector2 targetPos = hitTarget.transform.position;
            Vector2 dir = targetPos - originPos;
            float distance = Mathf.Abs(dir.magnitude);                      // 현재 몬스터와 용사의 거리

            if (distance > Mathf.Abs(attackRange)) continue;                  // 거리가 멀면 넘긴다.

            RaycastHit2D rayTarget = Physics2D.Raycast(originPos, dir, Mathf.Abs(attackRange), obstacleLayer);

            // '장애물이 존재하지 않'거나 '장애물이 존재해도 타겟 뒤에 있으면'
            if (!rayTarget || Vector2.Distance(rayTarget.transform.position, originPos) > distance)
            {
                AI.CanAttackHero = true;

                return;
            }
        }
    }
}