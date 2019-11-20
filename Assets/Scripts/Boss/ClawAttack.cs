using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawAttack : MonoBehaviour
{
    private enum State { before, attack, end }

    [SerializeField] private LayerMask attackLayer;
    [SerializeField] private Vector2 attackPos;
    [SerializeField] private Vector2 attackSize;
    [SerializeField] private int attackDamage = 30;

    private State state;

    public Vector2 AttackPos { set { attackPos = value; } }
    public Vector2 AttackSize { set { attackSize = value; } }

    private void Start()
    {
        StartCoroutine(AttackCycle());
    }

    private void FixedUpdate()
    {
        switch (state)
        {
            case State.before:
                Warning();
                break;
            case State.attack:
                Attack();
                break;
            case State.end:
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(attackPos, attackSize);
    }

    private void Warning()
    {
        GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.3f);
    }

    private void Attack()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
        Collider2D collider = Physics2D.OverlapBox(attackPos, attackSize, 0, attackLayer);
        if (collider) ApplyDamage(collider);
    }

    private IEnumerator AttackCycle()
    {
        state = State.before;

        yield return new WaitForSeconds(1.5f);

        state = State.attack;

        yield return new WaitForFixedUpdate();

        state = State.end;

        yield return new WaitForSeconds(0.3f);

        Destroy(gameObject);
    }

    private void ApplyDamage(Collider2D coll)
    {
        GameObject hero = coll.gameObject;
        hero.GetComponent<PlayerInput>().GetDamage(attackDamage, gameObject);
    }

}
