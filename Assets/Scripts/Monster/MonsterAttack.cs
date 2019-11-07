using System.Collections;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    [Header("Attack")]
    [SerializeField] private WeaponDelay delay;
    [SerializeField] private int attackDamage = 5;
    [SerializeField] private LayerMask attackLayer;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private Vector2 attackAreaPos;
    [SerializeField] private Vector2 attackArea;

    protected MonsterAI AI;
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
        AI = GetComponent<MonsterAI>();
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
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
        AI.FreezeState = true;
        animator.SetBool("Attack", true);

        yield return new WaitForSeconds(delay.pre);

        doingAttack = true;

        yield return new WaitForSeconds(delay.attack);

        doingAttack = false;
        animator.SetBool("Attack", false);

        yield return new WaitForSeconds(delay.post);

        AI.FreezeState = false;
        canAttack = true;
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