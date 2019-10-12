using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int damage = 5;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 700f;

    public int MaxHealth { get { return maxHealth; } }
    public int Damage { get { return damage; } }
    public float Speed { get { return speed; } }
    public float JumpForce { get { return jumpForce; } }
}
