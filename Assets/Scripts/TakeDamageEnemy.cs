using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeDamageEnemy : MonoBehaviour
{
    public float health = 50;
    public bool dead = false;
    private Animator enemyAnimator;

    void Start()
    {
      enemyAnimator = GetComponent<Animator>();
    }

    public void TakeDamage(float amount) {
      if(health > 0) {
        if (!enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Attack")) {
          enemyAnimator.CrossFade("Hit", 0.1f);
        }
        health -= amount;
      } else {
        Die();
      }
    }

    void Die() {
      if(!dead) {
        GetComponent<UnityEngine.AI.NavMeshAgent>().enabled = false;
        GameObject.Find("Character_Hero_Knight_Male").GetComponent<TakeDamagePlayer>().GiveHealth(health / 2);
        enemyAnimator.CrossFade("Death", 0.5f);
        dead = true;
      }
    }
}
