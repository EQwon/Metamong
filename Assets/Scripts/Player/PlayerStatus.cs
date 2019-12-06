using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerStatus : MonoBehaviour
{
    [Header("Status")]
    [SerializeField] private int maxHealth = 100;
    [SerializeField] private int damage = 5;
    [SerializeField] private float attackSpeed = 1f;
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
    private bool isFulfilling = false;

    public int MaxHealth { get { return maxHealth; } set { maxHealth = value; } }
    public int Damage { get { return damage; } set { damage = value; } }
    public float AttackSpeed { get { return attackSpeed; } set { attackSpeed = value; } }
    public float Speed { get { return speed; } set { speed = value; } }
    public float MovementDamping { set { movementDamping = value; } }
    public float JumpForce { get { return jumpForce; } set { jumpForce = value; } }
    public float InvincibleTime { set { invincibleTime = value; } }
    public float KnockBackForce { set { knockBackForce = value; } }

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
        if (SceneManager.GetActiveScene().name.Contains("Village"))
        {
            lastVillage = SceneManager.GetActiveScene().buildIndex;
        }
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        Contract.instance.KillContractCheck();
    }

    private void Update()
    {
        if (effectQueue.Count > 0 && isFulfilling == false)
            StartCoroutine(ContractFulfillment());
    }

    public void UpdateStatus()
    {
        //Debug.Log("스탯 업데이트");

        if (input.MaxHealth > maxHealth) AddEffectQueue(new ContractEffect(ResultClass.MaxHealth, maxHealth - input.MaxHealth, false));
        else if (input.MaxHealth < maxHealth) AddEffectQueue(new ContractEffect(ResultClass.MaxHealth, maxHealth - input.MaxHealth, true));
        input.MaxHealth = maxHealth;

        if (attacker.Damage > damage) AddEffectQueue(new ContractEffect(ResultClass.AttackDamage, damage - attacker.Damage, false));
        else if(attacker.Damage < damage) AddEffectQueue(new ContractEffect( ResultClass.AttackDamage, damage - attacker.Damage, true));
        attacker.Damage = damage;

        if (attacker.AttackSpeed > attackSpeed) AddEffectQueue(new ContractEffect(ResultClass.AttackSpeed, attackSpeed - attacker.AttackSpeed, false));
        else if (attacker.AttackSpeed < attackSpeed) AddEffectQueue(new ContractEffect(ResultClass.AttackSpeed, attackSpeed - attacker.AttackSpeed, true));
        attacker.AttackSpeed = attackSpeed;

        if (mover.Speed > speed) AddEffectQueue(new ContractEffect(ResultClass.Speed, speed - mover.Speed, false));
        else if (mover.Speed < speed) AddEffectQueue(new ContractEffect(ResultClass.Speed, speed - mover.Speed, true));
        mover.Speed = speed;

        mover.MovementDamping = movementDamping;
        mover.JumpForce = jumpForce;
        input.InvincibleTime = invincibleTime;
        mover.KnockBackForce = knockBackForce;
    }

    private void AddEffectQueue(ContractEffect contractEffect)
    {
        effectQueue.Add(contractEffect);
    }

    private IEnumerator ContractFulfillment()
    {
        isFulfilling = true;

        ResultClass target = effectQueue[0].resultClass;
        float value = effectQueue[0].value;
        bool isBuff = effectQueue[0].isBuff;

        Instantiate(contractEffectPrefab, transform);
        Instantiate(buffPrefab, UIManager.instance.transform).GetComponent<StatusEffect>().Set(target, value, isBuff);
        effectQueue.RemoveAt(0);

        yield return new WaitForSeconds(2f);

        if (effectQueue.Count > 0) StartCoroutine(ContractFulfillment());
        else isFulfilling = false;
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
