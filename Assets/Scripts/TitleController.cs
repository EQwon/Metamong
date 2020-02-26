using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{
    [SerializeField] private GameObject sparkling;
    [SerializeField] private List<RectTransform> spawnPos;

    [Header("Ask Panel")]
    [SerializeField] private GameObject askPanel;
    [SerializeField] private Text askText;

    private void Start()
    {
        askPanel.SetActive(false);
        StartCoroutine(Sparkle());
    }

    private IEnumerator Sparkle()
    {
        RectTransform pos = spawnPos[Random.Range(0, spawnPos.Count)];
        Instantiate(sparkling, pos.position, Quaternion.identity, transform);

        yield return new WaitForSeconds(Random.Range(1f, 3f));

        StartCoroutine(Sparkle());
    }

    public void StartGame()
    {
        if (Contract.instance)
        {
            askText.text = "챕터 " + (Contract.instance.ContractLevel + 1).ToString() + "부터 시작하시겠습니까?";
            askPanel.SetActive(true);
        }
        else
        {
            StartNew();
        }
    }

    public void StartNew()
    {
        SceneManager.LoadScene(1);
    }

    public void StartFollowing()
    {
        int chapter = Contract.instance.ContractLevel;

        if (chapter == 0) SceneManager.LoadScene(2);
        else if (chapter == 1) SceneManager.LoadScene(5);
        else if (chapter == 2) SceneManager.LoadScene(8);
    }
}
