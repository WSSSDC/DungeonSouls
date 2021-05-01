using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FancyNameMovement : MonoBehaviour
{
    RectTransform bigboobies;

    void Start()
    {
        bigboobies = gameObject.GetComponent<RectTransform>();
        StartCoroutine(enu());
        IEnumerator enu()
        {
            for (; ; ) {
   
                StartCoroutine(enumerator());
                yield return new WaitForSeconds(2.5f);
            }
        }
        IEnumerator enumerator()
        {
            for (int i = -50; i > -100; i--)
            {
                yield return new WaitForSeconds(0.05f);
                bigboobies.anchoredPosition3D = new Vector3(321, i, 0);
                //Debug.Log("A " + i);
            }

            for (int i = -100; i < -50; i++)
            {
                yield return new WaitForSeconds(0.05f);
                bigboobies.anchoredPosition3D = new Vector3(321, i, 0);
                //Debug.Log("A " + i);
            }
        }
    }

    void Update()
    {
        
    }
}
