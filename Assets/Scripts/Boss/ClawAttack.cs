using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ClawDirection { Right, Left }

public class ClawAttack : MonoBehaviour
{
    private enum State { before, attack, end }

    [SerializeField] private LayerMask attackLayer;
    [SerializeField] private Vector2 attackPos;
    [SerializeField] private Vector2 attackSize;
    [SerializeField] private int attackDamage = 30;
    [SerializeField] private ClawDirection dir;
    [SerializeField] private float warningTime = 0.4f;
    [SerializeField] private float waitingTime = 0.5f;

    private State state;

    public Vector2 AttackPos { set { attackPos = value; } }
    public Vector2 AttackSize { set { attackSize = value; } }
    public ClawDirection Direction { set { dir = value; } }

    private void Start()
    {
        if (dir == ClawDirection.Left) transform.localScale = new Vector3(-1, 1, 1);
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

    private void Warning()
    {
    }

    private void Attack()
    {
        Collider2D collider = Physics2D.OverlapBox(attackPos, attackSize, 0, attackLayer);
        if (collider) ApplyDamage(collider);
    }

    private IEnumerator AttackCycle()
    {
        state = State.before;

        yield return new WaitForSeconds(warningTime);

        state = State.attack;

        yield return new WaitForFixedUpdate();

        state = State.end;

        yield return new WaitForSeconds(waitingTime);

        Destroy(gameObject);
    }

    private void ApplyDamage(Collider2D coll)
    {
        GameObject hero = coll.gameObject;
        hero.GetComponent<PlayerInput>().GetDamage(attackDamage, gameObject);
    }
}
