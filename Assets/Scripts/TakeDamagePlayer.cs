using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TakeDamagePlayer : MonoBehaviour
{
    public float health = 100;
    public bool dead = false;
    private Animator animator;
    private GameObject healthSlider;

    void Start()
    {
      animator = GetComponent<Animator>();
      healthSlider = GameObject.Find("Slider");
    }

    public void GiveHealth(float amount) {
      health += amount;
      if(health > 100) {
        health = 100;
      }
      healthSlider.GetComponent<Slider>().value = health;
    }

    public void TakeDamage(float amount) {
      if(health > 0) {
        if (!animator.GetCurrentAnimatorStateInfo(0).IsName("Slash")) {
          animator.CrossFade("Hit", 0.1f);
        }
        health -= amount;
        healthSlider.GetComponent<Slider>().value = health;
      } else {
        StartCoroutine(Die());
      }
    }

    IEnumerator Die() {
      if(!dead) {
        animator.CrossFade("Death", 0.3f);
        dead = true;
        yield return new WaitForSeconds(1f);
        Application.LoadLevel(Application.loadedLevel);
      } else {
        yield return new WaitForSeconds(0.1f);
        if(!animator.GetCurrentAnimatorStateInfo(0).IsName("Death")) {
          dead = false;
        }
      }
    }
}
