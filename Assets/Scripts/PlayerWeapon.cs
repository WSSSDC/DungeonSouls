using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public float damage = 15;
    
    void OnTriggerEnter(Collider other) {
        if(other.tag == "Enemy") {
          other.GetComponent<TakeDamageEnemy>().TakeDamage(damage);
        }
    }
}
