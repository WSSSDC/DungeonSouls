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
        //Cursor.visible = false; This bullshit doesn't work, instead I replaced it with:
        Cursor.lockState = CursorLockMode.Locked;
        //Which actually works. Thanks for understanding!
        //Pedram's Note: It still doesn't work properlly. It's probably that you fucked up the controller
        //and it doesn't track the mouse correctly.
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
        bool mouseClick = Input.GetMouseButtonDown(0);
        bool isAiming = Input.GetMouseButtonDown(1); //Right Click Capture For Aiming The Bow

        if(mouseClick) {
          _animController.CrossFade("Slash", 0.5f);
        }

        if (isAiming)
        {
            _animController.SetBool("isAiming", true);
            Debug.Log("Aiming");
        }
        else
        {
            _animController.SetBool("isAiming", false);
            Debug.Log("Not Aiming");
        }

        if (leftShiftDown) {
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
