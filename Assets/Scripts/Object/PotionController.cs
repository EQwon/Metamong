using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionController : MonoBehaviour
{
    [SerializeField] private int healAmount = 30;
    [SerializeField] private AudioClip healingClip;

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.transform.tag == "Player")
        {
            PlayerInput player = coll.transform.GetComponent<PlayerInput>();
            player.GetHeal(healAmount);
            SoundManager.instance.PlaySE(healingClip);

            Destroy(gameObject);
        }
    }
}
