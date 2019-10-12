﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class WeaponDelay
{
    public float pre;
    public float post;
}

public class PlayerAttack : MonoBehaviour
{
    public WeaponDelay delay;
    private int damage;
    [SerializeField] private LayerMask attackLayer;

    private int isFacingRight = 1;
    private bool canAttack = true;

    private Vector2 attackPos { get { return new Vector2(isFacingRight * 1.2f, -0.2f) + (Vector2)transform.position; } }
    private Vector2 attackSize { get { return new Vector2(1.6f, 1.6f); } }

    private void Awake()
    {
        PlayerStatus stat = GetComponent<PlayerStatus>();

        damage = stat.Damage;
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

            enemy.GetComponent<MonsterAI>().GetDamage(damage);
        }
    }
}

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
        float rectWidth = (position.width - 10) / 2;
        var preRect = new Rect(position.x, position.y, rectWidth, position.height);
        var postRect = new Rect(position.x + rectWidth + 10, position.y, rectWidth, position.height);

        // Draw fields - passs GUIContent.none to each so they are drawn without labels
        EditorGUI.PropertyField(preRect, property.FindPropertyRelative("pre"), GUIContent.none);
        EditorGUI.PropertyField(postRect, property.FindPropertyRelative("post"), GUIContent.none);

        // Set indent back to what it was
        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}
