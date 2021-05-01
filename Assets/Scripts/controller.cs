using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField]
    private bool mouse = false;

    [SerializeField]
    private float _moveSpeed = 5f;

    [SerializeField]
    private float _gravity = 9.81f;

    [SerializeField]
    private float _sprintSpeed = 10f;

    [SerializeField]
    private float _turnSpeed = 10f;

    [SerializeField]
    private Animator _animController;

    private CharacterController _controller;




    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    bool _leftShift = false;

    void Update()
    {
        float horizontalInput = Input.GetAxis(mouse ? "Mouse X" : "Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float speed = _moveSpeed;
        bool leftShiftDown = Input.GetKeyDown(KeyCode.LeftShift);
        bool leftShiftUp = Input.GetKeyUp(KeyCode.LeftShift);

        if(leftShiftDown) {
          _leftShift = true;
          _animController.SetBool("isRunning", true);
          speed = _sprintSpeed;
        }

        if(leftShiftUp) {
          _leftShift = false;

          speed = _moveSpeed;
        }

        _animController.SetBool("isRunning", _leftShift);
        speed = _leftShift ? _sprintSpeed : _moveSpeed;

        Vector3 direction = transform.forward;

        transform.Rotate(0, _turnSpeed * Time.deltaTime * horizontalInput, 0);

        direction.y -= _gravity * Time.deltaTime;

        direction.x *= verticalInput;
        direction.z *= verticalInput;

        if(!_controller.isGrounded) {
          direction.x = 0f;
          direction.z = 0f;
        }

        _controller.Move(direction * speed * Time.deltaTime);

        _animController.SetBool("isWalking", Mathf.Abs(verticalInput) > 0);
    }
}
