using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private LayerMask attackLayer;
    [SerializeField] private LayerMask obstacleLayer;
    private int damage;
    
    public LayerMask AttackLayer { set { attackLayer = value; } }
    public int Damage { set { damage = value; } }

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
