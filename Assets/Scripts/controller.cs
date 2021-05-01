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
        Cursor.lockState = CursorLockMode.Locked;

        _controller = GetComponent<CharacterController>();
    }

    bool _leftShift = false;

    void Update()
    {
        float horizontalInput = Input.GetAxis("Mouse X");
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInputKeys = Input.GetAxis("Horizontal");
        float speed = _moveSpeed;
        bool leftShiftDown = Input.GetKeyDown(KeyCode.LeftShift);
        bool leftShiftUp = Input.GetKeyUp(KeyCode.LeftShift);
        bool mouseClick = Input.GetMouseButtonDown(0);
        bool isAiming = false; //Right Click Capture For Aiming The Bow
        bool blockDown = Input.GetMouseButtonDown(1);
        bool blockUp = Input.GetMouseButtonUp(1);

        if(blockDown) {
          _animController.SetBool("isBlocking", true);
        }

        if (blockUp) {
          _animController.SetBool("isBlocking", false);
        }

        if(mouseClick) {
          _animController.CrossFade("Slash", 0.5f);
        }

        if (isAiming)
        {
            _animController.SetBool("isAiming", true);
        }
        else
        {
            _animController.SetBool("isAiming", false);
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

        //Vector3 direction = transform.forward;
        Vector3 direction = transform.forward * verticalInput + Quaternion.AngleAxis(90, Vector3.up) * transform.forward * horizontalInputKeys / 1.5f;
        transform.Rotate(0, _turnSpeed * Time.deltaTime * horizontalInput, 0);

        direction.y -= _gravity * Time.deltaTime;

        //direction.x *= verticalInput;
        //direction.z *= verticalInput;

        if(!_controller.isGrounded || _animController.GetCurrentAnimatorStateInfo(0).IsName("Slash") || _animController.GetCurrentAnimatorStateInfo(0).IsName("BlockIdle")) {
          direction.x = 0f;
          direction.z = 0f;
        }

        // if(Input.GetAxis("Horizontal") < 0 && transform.rotation.y > 0){
        //   direction = Vector3.left;
        // }
        // else if(Input.GetAxis("Horizontal") < 0 && transform.rotation.y < 0){
        //   direction = Vector3.right;
        // }
        // else if(Input.GetAxis("Horizontal") > 0 && transform.rotation.y > 0){
        //   direction = Vector3.right;
        // }
        // else if(Input.GetAxis("Horizontal") > 0 && transform.rotation.y < 0){
        //   direction = Vector3.left;
        // }
        _controller.Move(direction * speed * Time.deltaTime);

        _animController.SetBool("isWalking", Mathf.Abs(verticalInput) > 0);
        _animController.SetBool("isStrafing", Mathf.Abs(horizontalInputKeys) > 0);
    }
}
