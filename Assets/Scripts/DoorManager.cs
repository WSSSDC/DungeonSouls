using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    private bool _interacting = false;
    [SerializeField]
    private float degrees = 90f;

    void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            _interacting = true;
        }
        OpenDoor();
    }

    void OpenDoor()
    {
        Quaternion target = Quaternion.Euler(0, degrees, 0);
        if (_interacting)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, target,  Time.deltaTime);
        }

        if (transform.rotation == target)
        {
            _interacting = false;
        }
    }
}
