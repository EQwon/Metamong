using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonNeedleRain : MonoBehaviour
{
    [SerializeField] private GameObject needlePrefab;
    [SerializeField] private float speed = 2f;
    [SerializeField] private int damage = 10;

    private float term = 0.1f;

    private void Start()
    {
        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        yield return new WaitForSeconds(2f);

        for(int i = 0; i < 40; i++)
        {
            float angle = Random.Range(220, 320);
            float rad = Mathf.Deg2Rad * angle;

            yield return new WaitForSeconds(term);
            GameObject needle = Instantiate(needlePrefab, transform.position, Quaternion.Euler(0, 0, angle + 90f));

            needle.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * speed;
            needle.GetComponent<ProjectileController>().Damage = damage;
        }

        yield return new WaitForSeconds(2f);
    }
}
