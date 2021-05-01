using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FancyNameMovement : MonoBehaviour
{

    public Transform bruh;

    void Update()
    {
        bruh.position = new Vector3(0, 120, 0);
    }
}
