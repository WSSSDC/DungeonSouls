using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    [SerializeField]
    private float degrees = 90f;

    void Update()
    {        
        Vector3 to = new Vector3(0, degrees, 0);

        transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, to, Time.deltaTime);
    }
}
