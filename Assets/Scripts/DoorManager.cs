using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    private bool _interacting = false;
    [SerializeField]
    private float degrees = 90f;
    
    private GameObject[] gos;

    void Update()
    {
        gos = GameObject.FindGameObjectsWithTag("Player");
        if (Input.GetKey(KeyCode.E))
        {
            if (Vector3.Distance(gos[0].transform.position, transform.position) < 15)
            {
                _interacting = true;
            }
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
