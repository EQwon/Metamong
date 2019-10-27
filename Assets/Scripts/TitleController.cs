using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    [SerializeField] private GameObject sparkling;
    [SerializeField] private List<RectTransform> spawnPos;

    private void Start()
    {
        StartCoroutine(Sparkle());
    }

    private IEnumerator Sparkle()
    {
        RectTransform pos = spawnPos[Random.Range(0, spawnPos.Count)];
        Instantiate(sparkling, pos.position, Quaternion.identity, transform);

        yield return new WaitForSeconds(Random.Range(1f, 3f));

        StartCoroutine(Sparkle());
    }

    public void MoveScene()
    {
        SceneManager.LoadScene(1);
    }
}
