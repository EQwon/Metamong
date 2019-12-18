using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditController : MonoBehaviour
{
    [SerializeField] private RectTransform creditPanel;
    [SerializeField] private GameObject returnButton;

    private void Start()
    {
        SoundManager.instance.BossBGM();
        returnButton.SetActive(false);

        creditPanel.anchoredPosition = Vector2.zero;
        StartCoroutine(Credit());
    }

    private IEnumerator Credit()
    {
        int cnt = 0;

        while (cnt <= 9)
        {
            cnt += 1;

            while (creditPanel.anchoredPosition.x >= -2000 * cnt)
            {
                creditPanel.anchoredPosition -= new Vector2(2000 * Time.fixedDeltaTime, 0);
                yield return new WaitForFixedUpdate();
            }

            yield return new WaitForSeconds(4f);
        }

        returnButton.SetActive(true);
    }

    public void ReturnToTitle()
    {
        SceneManager.LoadScene(0);
    }
}
