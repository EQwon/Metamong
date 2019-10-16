using System.Collections;
using UnityEngine;

public class MonsterAttack : MonoBehaviour
{
    [SerializeField] private WeaponDelay delay;
    [SerializeField] private int attackDamage = 5;
    [SerializeField] private int touchDamage = 5;
    [SerializeField] private LayerMask attackLayer;

    private int isFacingRight = 1;
    private bool canAttack = true;
    private bool doingAttack = false;

    private Vector2 attackPos { get { return new Vector2(isFacingRight * 1.2f, -0.2f) + (Vector2)transform.position; } }
    private Vector2 attackSize { get { return new Vector2(1.6f, 1.6f); } }
    public int IsFacingRight { get { return isFacingRight; } }
    public int AttackDamage { get { return attackDamage; } }
    public LayerMask AttackLayer { get { return attackLayer; } }

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

        yield return new WaitForSeconds(delay.pre);

        doingAttack = true;

        yield return new WaitForSeconds(delay.attack);

        doingAttack = false;

        yield return new WaitForSeconds(delay.post);

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
        Debug.Log("Apply Damage to Hero");
        hero.GetComponent<PlayerInput>().GetDamage(attackDamage, gameObject);
    }

    public void FlipFacingDir()
    {
        isFacingRight *= -1;
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            coll.gameObject.GetComponent<PlayerInput>().GetDamage(touchDamage, gameObject);
        }
    }
}