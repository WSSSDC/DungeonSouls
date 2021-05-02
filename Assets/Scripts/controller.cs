using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private float _damage = 15f;

    [SerializeField]
    private Animator _animController;

    private CharacterController _controller;
    private GameObject shield;
    private MeshRenderer shieldRenderer;
    private GameObject sword;
    private MeshRenderer swordRenderer;
    private GameObject bow;
    private MeshRenderer bowRenderer;

    private GameObject crosshair;

    private TakeDamagePlayer takeDamagePlayer;
    private GameObject playerObject;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        shield = GameObject.Find("SM_Wep_Shield_Heater_02");
        shieldRenderer = shield.GetComponent<MeshRenderer>();
        sword = GameObject.Find("SM_Wep_GreatSword_01");

        swordRenderer = sword.GetComponent<MeshRenderer>();
        bow = GameObject.Find("LowPolyAssetsBow");
        bowRenderer = bow.GetComponent<MeshRenderer>();
        _controller = GetComponent<CharacterController>();

        crosshair = GameObject.Find("Crosshair");
        crosshair.active = false;

        playerObject = GameObject.Find("Character_Hero_Knight_Male");
        takeDamagePlayer = playerObject.GetComponent<TakeDamagePlayer>();
    }

    bool _leftShift = false;
    public bool bowMode = false;
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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene(0);
        }

        if(takeDamagePlayer.health > 0) {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
          bowMode = !bowMode;
          _animController.SetBool("isBowEquipped", bowMode);
          changeMode();
        }


        if (blockDown && bowMode)
        {
            crosshair.active = true;
            _animController.SetBool("isAiming", true);
        }

        if (blockUp && bowMode)
        {
            GameObject.Find("Crosshair").active = false;
            _animController.SetBool("isAiming", false);
        }

        if(blockDown && !bowMode) {
          _animController.SetBool("isBlocking", true);
        }

        if (blockUp && !bowMode) {
          _animController.SetBool("isBlocking", false);
        }

        if (mouseClick && !bowMode)
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
        }

        if (leftShiftDown && !bowMode) {
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



        if(!_controller.isGrounded || (slashState == SlashState.Slashing) || _animController.GetCurrentAnimatorStateInfo(0).IsName("BlockIdle")) {
          direction.x = 0f;
          direction.z = 0f;
        }


        _controller.Move(direction * speed * Time.deltaTime);

        _animController.SetBool("isWalking", Mathf.Abs(verticalInput) > 0);
        _animController.SetBool("isStrafing", Mathf.Abs(horizontalInputKeys) > 0);
      }

    }

    void changeMode() {
      shieldRenderer.enabled = !bowMode;
      swordRenderer.enabled = !bowMode;
      bowRenderer.enabled = bowMode;
    }

    IEnumerator SlashRoutine()
    {
        slashState = SlashState.Slashing;
        yield return new WaitForSeconds(0.4f);
        DoDamage();
        yield return new WaitForSeconds(0.9f);
        if (slashState == SlashState.Comboing) {
          _animController.CrossFade("Backhand", 0.2f);
          StartCoroutine(ComboRountine());
        } else {
          slashState = SlashState.None;
        }
    }

    IEnumerator ComboRountine()
    {
        yield return new WaitForSeconds(0.5f);
        slashState = SlashState.None;
    }

    void DoDamage() {
      GameObject[] gos;
      gos = GameObject.FindGameObjectsWithTag("Enemy");
      Vector3 position = transform.position;
      foreach (GameObject go in gos)
      {
          Vector3 diff = go.transform.position - position;
          float dist = diff.sqrMagnitude;
          if (dist < 4f && go.GetComponent<TakeDamageEnemy>() != null)
          {
              go.GetComponent<TakeDamageEnemy>().TakeDamage(_damage);
          }
      }
    }
}
