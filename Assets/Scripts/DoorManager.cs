using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    private enum doorType { SingleDoor, DoubleDoorL, DoubleDoorR};

    [SerializeField]
    private float degrees = 90f;

    [SerializeField]
    doorType type;

    void Update()
    {
        Vector3 to = new Vector3(0, degrees, 0);
        if (type == doorType.SingleDoor)
        {
            transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, to, Time.deltaTime);
        }
    }
}
