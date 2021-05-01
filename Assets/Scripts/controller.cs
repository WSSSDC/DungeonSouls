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

    public CharacterController _controller;

    private float _directionY;

    void Start()
    {
        
    }

    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(-horizontalInput, 0f, -verticalInput).normalized;
        
        _directionY -= _gravity * Time.deltaTime;
        if(Input.GetKeyDown(KeyCode.LeftShift)){
            direction.x = direction.x * _sprintMultiplier;
        }

        direction.y = _directionY;

        _controller.Move(direction * _moveSpeed * Time.deltaTime);
    }
}