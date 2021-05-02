using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{

    public float swordDamage = 15;

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnTriggerEnter(Collider other) {
        if(other.tag == "Enemy") {
          other.GetComponent<TakeDamageEnemy>().TakeDamage(swordDamage);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
