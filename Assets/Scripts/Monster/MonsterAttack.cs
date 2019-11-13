using System.Collections;
using UnityEngine;

public enum AttackState { None, Pre, Attack, Post }

public class MonsterAttack : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private WeaponDelay delay;
    [SerializeField] private int attackDamage = 5;
    [SerializeField] private float attackSpeed = 0f;
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
    private AttackState attackState = AttackState.None;

    private Vector2 attackPos { get { return new Vector2(isFacingRight * attackAreaPos.x, attackAreaPos.y) + (Vector2)transform.position; } }
    private Vector2 attackSize { get { return new Vector2(attackArea.x, attackArea.y); } }
    public int IsFacingRight { get { return isFacingRight; } }
    public int AttackDamage { get { return attackDamage; } }
    public LayerMask AttackLayer { get { return attackLayer; } }

    private void Awake()
    {
        AI = GetComponent<MonsterAI>();
        mover = GetComponent<MonsterMovement>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        switch (attackState)
        {
            case AttackState.Pre:
                mover.FacingTarget(AI.Player);
                mover.Move(0);
                AI.FreezeState = true;
                animator.SetBool("Attack", true);
                break;
            case AttackState.Attack:
                mover.Move(attackSpeed);
                DoAttack();
                break;
            case AttackState.Post:
                mover.Move(0);
                animator.SetBool("Attack", false);
                break;
            default:
                AI.FreezeState = false;
                break;
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
        if (attackState != AttackState.None) yield break;

        attackState = AttackState.Pre;

        yield return new WaitForSeconds(delay.pre);

        attackState = AttackState.Attack;

        yield return new WaitForSeconds(delay.attack);

        attackState = AttackState.Post;

        yield return new WaitForSeconds(delay.post);

        attackState = AttackState.None;
        AI.FreezeState = false;
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
}