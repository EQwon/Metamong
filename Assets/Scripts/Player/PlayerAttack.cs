﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class WeaponDelay
{
    public float pre;
    public float attack;
    public float post;
}

public class PlayerAttack : MonoBehaviour
{
    public WeaponDelay delay;
    [SerializeField] private int damage;
    [SerializeField] private LayerMask attackLayer;
    [SerializeField] private GameObject attackEffect;

    [Header("SE")]
    [SerializeField] private AudioClip attackClip;
    [SerializeField] private AudioClip hitClip;

    private int isFacingRight = 1;
    private bool canAttack = true;
    private Animator animator;

    private Vector2 attackPos { get { return new Vector2(isFacingRight * 0.6f, -0.1f) + (Vector2)transform.position; } }
    private Vector2 attackSize { get { return new Vector2(1.7f, 1.2f); } }

    public int Damage { get { return damage; } set { damage = value; } }
    public float AttackSpeed { get { return 1 / (delay.pre + delay.post); } set { delay.post = (1 / value) - delay.pre; } }

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
    }

    public void Attack(float move, bool attack)
    {
        if (move > 0) isFacingRight = 1;
        else if (move < 0) isFacingRight = -1;

        if (canAttack && attack)
        {
            StartCoroutine(Attack());
        }
    }

    private IEnumerator Attack()
    {
        if (canAttack == false) yield break;

        canAttack = false;
        animator.SetTrigger("Attack");
        StartCoroutine(SoundManager.instance.PlaySE(attackClip, 0.1f));

        yield return new WaitForSeconds(delay.pre);

        Collider2D[] colliders = Physics2D.OverlapBoxAll(attackPos, attackSize, 0, attackLayer);
        ApplyDamage(colliders);

        yield return new WaitForSeconds(delay.post);

        canAttack = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackPos, attackSize);
    }

    private void ApplyDamage(Collider2D[] colliders)
    {
        foreach (Collider2D coll in colliders)
        {
            GameObject enemy = coll.gameObject;

            if (enemy.tag == "Boss") enemy.GetComponent<BossAI>().GetDamage(damage);
            else enemy.GetComponent<MonsterAI>().GetDamage(damage, gameObject);

            SoundManager.instance.PlaySE(hitClip);
            Instantiate(attackEffect, attackPos, Quaternion.identity);
            StartCoroutine(Camera.main.gameObject.GetComponent<CameraWalk>().Shaking());
        }
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(WeaponDelay))]
public class DelayDrawerUIE : PropertyDrawer
{
    // Draw the property inside the given rect
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Using BeginProperty / EndProperty on the parent property means that
        // prefab override logic works on the entire property.
        EditorGUI.BeginProperty(position, label, property);

        // Draw label
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        // Don't make child fields be indented
        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Calculate rects
        float rectWidth = (position.width - 10) / 3;
        var preRect = new Rect(position.x, position.y, rectWidth, position.height);
        var attackRect = new Rect(position.x + rectWidth + 5, position.y, rectWidth, position.height);
        var postRect = new Rect(position.x + rectWidth * 2 + 10, position.y, rectWidth, position.height);

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(preRect, property.FindPropertyRelative("pre"), GUIContent.none);
        EditorGUI.PropertyField(attackRect, property.FindPropertyRelative("attack"), GUIContent.none);
        EditorGUI.PropertyField(postRect, property.FindPropertyRelative("post"), GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
#endif