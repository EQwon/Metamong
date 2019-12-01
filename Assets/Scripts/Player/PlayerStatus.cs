using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatus : MonoBehaviour
{
    [Header("Status")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int damage = 5;
    [SerializeField] private float attackPostDelay = 1f;
    [SerializeField] private float speed = 5f;
    [SerializeField] private float movementDamping = 0.05f;
    [SerializeField] private float jumpForce = 700f;
    [SerializeField] private float invincibleTime;
    [SerializeField] private float knockBackForce;
    [SerializeField] private int lastVillage;

    [Header("Buff")]
    [SerializeField] private GameObject buffPrefab;

    public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
    public int Damage { get { return damage; } set { damage = value; } }
    public float AttackPostDelay { set { attackPostDelay = value; } }
    public float AttackSpeed { get { return 1 / (0.1f + attackPostDelay); } }
    public float Speed { get { return speed; } set { speed = value; } }
    public float MovementDamping { set { movementDamping = value; } }
    public float JumpForce { get { return jumpForce; } set { jumpForce = value; } }
    public float InvincibleTime { set { invincibleTime = value; } }
    public float KnockBackForce { set { knockBackForce = value; } }
    public int LastVillage { get { return lastVillage; } }

    private PlayerInput input;
    private PlayerMovement mover;
    private PlayerAttack attacker;

    public static PlayerStatus instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        input = GetComponent<PlayerInput>();
        mover = GetComponent<PlayerMovement>();
        attacker = GetComponent<PlayerAttack>();

        UpdateStatus();
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoad;
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        Contract.instance.KillContractCheck();
    }

    public void UpdateStatus()
    {
        //Debug.Log("스탯 업데이트");

        if (input.MaxHealth > maxHealth) Debuff(ResultClass.MaxHealth, maxHealth - input.MaxHealth);
        else if (input.MaxHealth < maxHealth) Buff(ResultClass.MaxHealth, maxHealth - input.MaxHealth);
        input.MaxHealth = maxHealth;

        if (attacker.Damage > damage) Debuff(ResultClass.AttackDamage, damage - attacker.Damage);
        else if(attacker.Damage < damage) Buff(ResultClass.AttackDamage, damage - attacker.Damage);
        attacker.Damage = damage;

        //if (attacker.AttackPostDelay > attackPostDelay) Buff();
        //else if (attacker.AttackPostDelay < attackPostDelay) Debuff();
        attacker.AttackPostDelay = attackPostDelay;

        if (mover.Speed > speed) Debuff(ResultClass.Speed, speed - mover.Speed);
        else if (mover.Speed < speed) Buff(ResultClass.Speed, speed - mover.Speed);
        mover.Speed = speed;

        mover.MovementDamping = movementDamping;
        mover.JumpForce = jumpForce;
        input.InvincibleTime = invincibleTime;
        mover.KnockBackForce = knockBackForce;
    }

    private void Buff(ResultClass target, float value)
    {
        Instantiate(buffPrefab, UIManager.instance.transform).GetComponent<StatusEffect>().Set(target, value, true);
    }

    private void Debuff(ResultClass target, float value)
    {
        Instantiate(buffPrefab, UIManager.instance.transform).GetComponent<StatusEffect>().Set(target, value, false);
    }
}
