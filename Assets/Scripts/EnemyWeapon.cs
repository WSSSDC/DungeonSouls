using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon : MonoBehaviour
{
  public float damage = 15;

  void OnTriggerEnter(Collider other) {
      if(other.tag == "Player") {
        other.GetComponent<TakeDamagePlayer>().TakeDamage(damage);
      }
  }
}
