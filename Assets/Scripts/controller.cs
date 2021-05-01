using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [SerializeField]
    private bool mouse = false;

    [SerializeField]
    private bool doorNearby = false;

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
    private bool bowMode = false;
    enum SlashState {None, Slashing, Comboing};
    SlashState slashState = SlashState.None;

    void Update()
    {
        float horizontalInput = Input.GetAxis("Mouse X");
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInputKeys = Input.GetAxis("Horizontal");
        float speed = _moveSpeed;
        bool leftShiftDown = Input.GetKeyDown(KeyCode.LeftShift);
        bool leftShiftUp = Input.GetKeyUp(KeyCode.LeftShift);
        bool mouseClick = Input.GetMouseButtonDown(0);
        bool blockDown = Input.GetMouseButtonDown(1);
        bool blockUp = Input.GetMouseButtonUp(1);
        bool isSlashing = _animController.GetCurrentAnimatorStateInfo(0).IsName("Slash");
        bool mouseIsClicking = false;

        if (Input.GetKey(KeyCode.Tab))
        {
            Debug.Log("Modes Switched" + bowMode);
            if (!bowMode)
            {
                bowMode = true;
                
                _animController.SetBool("isBowEquipped", true);
                
            }
            else
            {
                bowMode = false;
                _animController.SetBool("isBowEquipped", false);
            }
        }
        if (Input.GetMouseButtonDown(1) && bowMode)
        {
            _animController.SetBool("isAiming", true);
        }
        if (Input.GetMouseButtonUp(1) && bowMode)
        {
            _animController.SetBool("isAiming", false);
        }
        
        if(blockDown) {
          _animController.SetBool("isBlocking", true);
        }

        if (blockUp) {
          _animController.SetBool("isBlocking", false);
        }

        if (mouseClick)
        {
            if (slashState == SlashState.Slashing)
            {
                slashState = SlashState.Comboing;
            }
            else if (slashState == SlashState.None)
            {
                _animController.CrossFade("Slash", 0.2f);
                StartCoroutine(SlashRoutine());
            }
            else
            {
                if (_animController.GetCurrentAnimatorStateInfo(0).IsName("Backhand"))
                {

                }
            }
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

        if (doorNearby)
        {
            if (Input.GetKey("E"))
            {
                //Run The Door Open Script
            }
        }

        _animController.SetBool("isRunning", _leftShift);
        speed = _leftShift ? _sprintSpeed : _moveSpeed;

        //Vector3 direction = transform.forward;
        Vector3 direction = transform.forward * verticalInput + Quaternion.AngleAxis(90, Vector3.up) * transform.forward * horizontalInputKeys / 1.5f;
        transform.Rotate(0, _turnSpeed * Time.deltaTime * horizontalInput, 0);

        direction.y -= _gravity * Time.deltaTime;

        //direction.x *= verticalInput;
        //direction.z *= verticalInput;



        if(!_controller.isGrounded || isSlashing || _animController.GetCurrentAnimatorStateInfo(0).IsName("BlockIdle")) {
          direction.x = 0f;
          direction.z = 0f;
        }


        _controller.Move(direction * speed * Time.deltaTime);

        _animController.SetBool("isWalking", Mathf.Abs(verticalInput) > 0);
        _animController.SetBool("isStrafing", Mathf.Abs(horizontalInputKeys) > 0);

    }

    IEnumerator SlashRoutine()
    {
        slashState = SlashState.Slashing;
        //yield return new WaitForSeconds(0.3f);
        yield return new WaitForSeconds(1.3f);
        Debug.Log(slashState);
        if (slashState == SlashState.Comboing) {
          _animController.CrossFade("Backhand", 0.2f);
          StartCoroutine(ComboRountine());
        } else {
          slashState = SlashState.None;
        }
    }

    IEnumerator ComboRountine()
    {
        yield return new WaitForSeconds(0.3f);
        slashState = SlashState.None;
    }
}
