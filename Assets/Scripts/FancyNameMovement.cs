using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FancyNameMovement : MonoBehaviour
{
    public Transform bruh;

    RectTransform bigboobies;

    void Start()
    {
        bigboobies = gameObject.GetComponent<RectTransform>();
    }

    void Update()
    {
        StartCoroutine(enumerator());
        IEnumerator enumerator()
        {
            for (int i = 175; i > 150; i--)
            {
                bigboobies.anchoredPosition3D = new Vector3(0, i, 0);
                yield return new WaitForSeconds(1f);
            }

            for (int i = 150; i < 175; i++)
            {
                bigboobies.anchoredPosition3D = new Vector3(0, i, 0);
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
