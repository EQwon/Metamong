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
    [SerializeField] private GameObject contractEffectPrefab;
    private List<ContractEffect> effectQueue = new List<ContractEffect>();

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

        if (input.MaxHealth > maxHealth) ContractFulfillment(new ContractEffect(ResultClass.MaxHealth, maxHealth - input.MaxHealth, false));
        else if (input.MaxHealth < maxHealth) ContractFulfillment(new ContractEffect(ResultClass.MaxHealth, maxHealth - input.MaxHealth, true));
        input.MaxHealth = maxHealth;

        if (attacker.Damage > damage) ContractFulfillment(new ContractEffect(ResultClass.AttackDamage, damage - attacker.Damage, false));
        else if(attacker.Damage < damage) ContractFulfillment(new ContractEffect( ResultClass.AttackDamage, damage - attacker.Damage, true));
        attacker.Damage = damage;

        if (attacker.AttackPostDelay > attackPostDelay) ContractFulfillment(new ContractEffect(ResultClass.AttackSpeed, attackPostDelay - attacker.AttackPostDelay, true));
        else if (attacker.AttackPostDelay < attackPostDelay) ContractFulfillment(new ContractEffect(ResultClass.AttackSpeed, attackPostDelay - attacker.AttackPostDelay, false));
        attacker.AttackPostDelay = attackPostDelay;

        if (mover.Speed > speed) ContractFulfillment(new ContractEffect(ResultClass.Speed, speed - mover.Speed, false));
        else if (mover.Speed < speed) ContractFulfillment(new ContractEffect(ResultClass.Speed, speed - mover.Speed, true));
        mover.Speed = speed;

        mover.MovementDamping = movementDamping;
        mover.JumpForce = jumpForce;
        input.InvincibleTime = invincibleTime;
        mover.KnockBackForce = knockBackForce;
    }

    private void ContractFulfillment(ContractEffect contractEffect)
    {
        ResultClass target = contractEffect.resultClass;
        float value = contractEffect.value;
        bool isBuff = contractEffect.isBuff;

        Instantiate(contractEffectPrefab, transform);
        Instantiate(buffPrefab, UIManager.instance.transform).GetComponent<StatusEffect>().Set(target, value, isBuff);
    }

    private struct ContractEffect
    {
        public ResultClass resultClass;
        public float value;
        public bool isBuff;

        public ContractEffect(ResultClass resultClass, float value, bool isBuff)
        {
            this.resultClass = resultClass;
            this.value = value;
            this.isBuff = isBuff;
        }
    }
}
