using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SignDirection { RightUp, Right, RightDown, LeftDown, Left, LeftUp }

public class SignController : MonoBehaviour
{
    [SerializeField] private GameObject signPrefab;
    [SerializeField] private List<SignDirection> directions;

    [Header("Resources Holder")]
    [SerializeField] private Sprite longBar;
    [SerializeField] private List<Sprite> signSprites;

    private List<float> posY = new List<float> { -0.3f, -1.1f, -1.9f };

    private void Start()
    {
        for (int i = 0; i < directions.Count; i++)
        {
            GameObject sign = Instantiate(signPrefab, transform);
            sign.transform.localPosition = new Vector2(0, posY[i]);
            sign.GetComponent<SpriteRenderer>().sprite = signSprites[(int)directions[i]];
        }

        if (directions.Count == 3)
        {
            GetComponent<SpriteRenderer>().sprite = longBar;
            transform.position += new Vector3(0, 1f, 0);
        }
    }
}
