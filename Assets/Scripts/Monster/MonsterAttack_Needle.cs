using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack_Needle : MonsterAttack
{
    [Header("Poison Sting")]
    [SerializeField] private GameObject needlePrefab;
    [SerializeField] private Vector2 firePos;
    [SerializeField] private float fireSpeed;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawSphere((Vector2)transform.position + firePos, 0.1f);
    }

    public override void DoAttack()
    {
        Vector2 spawnPos = (Vector2)transform.position + (IsFacingRight > 0 ? firePos : firePos * new Vector2(-1, 1));

        Vector2 playerPos = AI.Player.transform.position;
        Vector2 dir = playerPos - (Vector2)transform.position;

        Vector2 fireVelocity = dir.normalized * fireSpeed;

        GameObject needle = Instantiate(needlePrefab, spawnPos, Quaternion.Euler(0, 0, Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg));
        needle.GetComponent<Rigidbody2D>().velocity = fireVelocity;
        needle.GetComponent<ProjectileController>().Damage = AttackDamage;
        needle.GetComponent<ProjectileController>().AttackLayer = AttackLayer;
    }
}
