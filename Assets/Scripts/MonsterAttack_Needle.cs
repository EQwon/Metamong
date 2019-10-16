using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterAttack_Needle : MonsterAttack
{
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
        Vector2 spawnSpeed = IsFacingRight > 0 ? new Vector2(fireSpeed, 0) : new Vector2(-fireSpeed, 0);

        GameObject needle = Instantiate(needlePrefab, spawnPos, Quaternion.identity);
        needle.GetComponent<Rigidbody2D>().velocity = spawnSpeed;
        needle.GetComponent<ProjectileController>().Damage = AttackDamage;
        needle.GetComponent<ProjectileController>().AttackLayer = AttackLayer;
        needle.transform.rotation = IsFacingRight > 0 ? Quaternion.Euler(0, 0, 0) : Quaternion.Euler(0, 180, 0);
    }
}
