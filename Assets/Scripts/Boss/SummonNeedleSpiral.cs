using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonNeedleSpiral : MonoBehaviour
{
    [SerializeField] private GameObject needlePrefab;
    [SerializeField] private float speed = 6f;
    [SerializeField] private int lineNum;
    [SerializeField] private int damage = 10;

    private float term = 0.2f;

    private void Start()
    {
        StartCoroutine(Shoot());
    }

    private IEnumerator Shoot()
    {
        yield return new WaitForSeconds(1f);

        float rot = 0f;
        int dir = Random.Range(0, 100) >= 50 ? 1 : -1;

        for(int i = 0; i < 15; i++)
        {
            for (int j = 0; j < lineNum; j++)
            {
                float angle = rot + 360f * j / lineNum;
                float rad = Mathf.Deg2Rad * angle;

                GameObject needle = Instantiate(needlePrefab, transform.position, Quaternion.Euler(0, 0, angle + 90f));
                needle.GetComponent<Rigidbody2D>().velocity = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)) * speed;
                needle.GetComponent<ProjectileController>().Damage = damage;
            }
            yield return new WaitForSeconds(term);
            rot += 10f * dir;
        }

        yield return new WaitForSeconds(1f);
    }
}
