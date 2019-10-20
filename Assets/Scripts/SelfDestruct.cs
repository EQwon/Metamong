using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField] private float time;

    private void Start()
    {
        StartCoroutine(SelfDestroy());
    }

    private IEnumerator SelfDestroy()
    {
        yield return new WaitForSeconds(time);

        Destroy(gameObject);
    }
}
