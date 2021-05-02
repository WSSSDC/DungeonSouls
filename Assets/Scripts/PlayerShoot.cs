using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
  public GameObject arrow;
  public Transform shootPoint;
  public float shootSpeed;
  private GameObject character;

  void Start() {
    character = GameObject.Find("Character");
  }

  private void Update() {
      if(character.GetComponent<Controller>().bowMode && Input.GetMouseButtonDown(0)){
          GameObject current_arrow = Instantiate(arrow, shootPoint.position, shootPoint.rotation);
          Rigidbody rb = current_arrow.GetComponent<Rigidbody>();
          rb.AddForce(transform.forward * shootSpeed * Time.deltaTime, ForceMode.VelocityChange);
      }
  }
}
