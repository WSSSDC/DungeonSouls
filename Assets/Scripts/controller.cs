using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class controller : MonoBehaviour
{
    [SerializeField]
    private float _moveSpeed = 5f;
    
    [SerializeField]
    private float _gravity = 9.81f;

    [SerializeField]
    private float _sprintMultiplier = 1.5f;

    [SerializeField]
    private float _turnSpeed = 0.1f;

    private CharacterController _controller;

    private float _directionY;
    private float _turnSmoothVelocity;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(-horizontalInput, 0, -verticalInput);
        
        _directionY -= _gravity * Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.LeftShift)){
            direction.x = direction.x * _sprintMultiplier;
        }
        float _targetAngle = Mathf.Atan2(-direction.x, -direction.y) * Mathf.Rad2Deg;
        float _angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetAngle, ref _turnSmoothVelocity, _turnSpeed);

        direction.y = _directionY;

        transform.rotation = Quaternion.Euler(0f, _angle, 0f);
        _controller.Move(direction * _moveSpeed * Time.deltaTime);
        
    }
}