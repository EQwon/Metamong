using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MonsterBasicAttack : MonoBehaviour
{
    public WeaponDelay delay;
    public int damage;
    [SerializeField] private LayerMask attackLayer;

    private int isFacingRight = 1;
    private bool canAttack = true;

    private Vector2 attackPos { get { return new Vector2(isFacingRight * 1.2f, -0.2f) + (Vector2)transform.position; } }
    private Vector2 attackSize { get { return new Vector2(1.6f, 1.6f); } }

    private void FixedUpdate()
    {
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

        Collider2D collider = Physics2D.OverlapBox(attackPos, attackSize, 0, attackLayer);
        ApplyDamage(collider);

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

        hero.GetComponent<PlayerInput>().GetDamage(damage);
    }

    public void FlipFacingDir()
    {
        isFacingRight *= -1;
    }
}