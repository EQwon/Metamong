using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private LayerMask attackLayer;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private AudioClip instantiateClip;
    private int damage;
    
    public LayerMask AttackLayer { set { attackLayer = value; } }
    public int Damage { set { damage = value; } }

    private void Start()
    {
        SoundManager.instance.PlaySE_Volume(instantiateClip, 0.7f);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (obstacleLayer == (obstacleLayer | (1 << coll.gameObject.layer)))
        {
            Destroy(gameObject);
            return;
        }

        if (attackLayer == (attackLayer | (1 << coll.gameObject.layer)))
        {
            GameObject hero = coll.gameObject;

            hero.GetComponent<PlayerInput>().GetDamage(damage, gameObject);
            Destroy(gameObject);
        }
    }
}
