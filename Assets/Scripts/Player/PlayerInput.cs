using System.Collections;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] private int maxHealth;
    [SerializeField] private int health;
    [SerializeField] private float invincibleTime = 0.8f;

    private PlayerMovement mover;
    private PlayerAttack attacker;
    private Animator animator;
    private float horizontalMove = 0;
    private bool jump = false;
    private bool dash = false;
    private bool attack = false;
    private bool isInvincible = false;
    private bool isDead = false;
    private GameObject nowGate = null;
    private GameObject warningSign = null;
    private GameObject oldMan = null;

    public int MaxHealth
    {
        get { return maxHealth; }
        set
        {
            if (value > maxHealth)
                health += value - maxHealth;
            maxHealth = value;
            if (health > maxHealth)
                health = maxHealth;
        }
    }
    public int Health { get { return health; } set { health = value; } }
    public float InvincibleTime { set { invincibleTime = value; } }
    public GameObject NowGate { set { nowGate = value; } }
    public GameObject WarningSign { set { warningSign = value; } }
    public GameObject OldMan { set { oldMan = value; } }

    private void Awake()
    {
        mover = GetComponent<PlayerMovement>();
        attacker = GetComponent<PlayerAttack>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        Health = maxHealth;
    }

    private void Update()
    {
        if (isDead)
        {
            StartCoroutine(Dead());
            return;
        }

        horizontalMove = Input.GetAxisRaw("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(horizontalMove));
        UIManager.instance.AdjustingHealthBar(Health, MaxHealth);

        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("IsJumping", true);
        }
        if (Input.GetButtonDown("Attack"))
        {
            attack = true;
        }
        if (Input.GetButtonDown("Dash"))
        {
            dash = true;
        }
        if (Input.GetButtonDown("Action"))
        {
            if (nowGate) nowGate.GetComponent<GateController>().UseGate();
            if (warningSign) warningSign.GetComponent<WarningSignController>().ShowWarningPanel();
            if (oldMan) oldMan.GetComponent<OldManController>().StartContract();
        }
        if (Input.GetButtonDown("Contract"))
        {
            UIManager.instance.ContractPanel();
        }
        if (Input.GetButtonDown("Cancel"))
        {
            UIManager.instance.Menu();
        }
    }

    public void OnLanding()
    {
        animator.SetBool("IsJumping", false);
    }

    private void FixedUpdate()
    {
        mover.Move(horizontalMove, jump, dash);
        attacker.Attack(horizontalMove, attack);
        jump = false;
        attack = false;
        dash = false;

        if (health <= 0)
        {
            health = 0;
            isDead = true;
        }
    }

    public void GetDamage(int amount, GameObject attacker)
    {
        if (isInvincible) return;
        if (isDead) return;

        isInvincible = true;
        StartCoroutine(Invincible());

        health -= amount;

        Vector2 dir = Vector2.zero;
        dir.x = transform.position.x > attacker.transform.position.x ? 0.87f : -0.87f;
        dir.y = 0.3f;

        StartCoroutine(mover.KnockBack(dir));
    }

    public void GetHeal(int amount)
    {
        if (isDead) return;

        health += amount;
        if (health > MaxHealth) health = maxHealth;
    }

    private IEnumerator Invincible()
    {
        GetComponent<SpriteRenderer>().color = Color.grey;

        yield return new WaitForSeconds(invincibleTime);

        GetComponent<SpriteRenderer>().color = Color.white;

        isInvincible = false;
    }

    private IEnumerator Dead()
    {
        horizontalMove = 0;
        jump = false;
        attack = false;
        animator.SetBool("IsDead", true);

        yield return new WaitForSeconds(1.5f);

        UIManager.instance.Menu(true);
    }
}
