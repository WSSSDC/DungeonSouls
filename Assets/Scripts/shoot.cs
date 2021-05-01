using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shoot : MonoBehaviour {
    public GameObject Arrow;
    public Transform ShootPoint;
    public float shootSpeed;
    private void Update() {
        if(Input.GetButtonDown("Fire 1")){
            GameObject current_arrow = Instantiate(Arrow, ShootPoint.position, ShootPoint.rotation);
            Rigidbody rb = current_arrow.GetComponent<Rigidbody>();
            rb.AddForce(transform.forward * shootSpeed * Time.deltaTime, ForceMode.VelocityChange);
        }
    }
}