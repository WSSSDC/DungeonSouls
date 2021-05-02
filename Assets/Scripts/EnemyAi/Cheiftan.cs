using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TakeDamageEnemy;

public class Cheiftan : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent _agent;
    public LayerMask _whatIsGround, _whatIsPlayer;
    public float _sightRange, _attackRange, _moveSpeed, _wanderRange, _attackSpeed, _blockTendancy, _magicTimeout = 5, _projectileSpeed;
    public GameObject magic;
    public Transform _castPoint;
    private Animator enemyAnimator;
    private bool _playerInSight = false, _playerInRange = false;
    private Vector3 _wanderLocation;
    private float _attackTime;
    private Transform _player;
    private GameObject playerObject;
    private TakeDamageEnemy takeDamageEnemy;
    private float _time;
    private float _timeUntilMagic;


    void Start()
    {
      _attackTime = _attackSpeed;
      _player = GameObject.Find("Character").transform;
      playerObject = GameObject.Find("Character_Hero_Knight_Male");
      enemyAnimator = GetComponent<Animator>();
      takeDamageEnemy = GetComponent<TakeDamageEnemy>();
      _timeUntilMagic = _magicTimeout;
      _time = Time.time;
    }


    void Update()
    {
      if(!takeDamageEnemy.dead) {
        _timeUntilMagic -= Time.deltaTime;
        _playerInSight = Physics.CheckSphere(transform.position, _sightRange, _whatIsPlayer);
        _playerInRange = Physics.CheckSphere(transform.position, _attackRange, _whatIsPlayer);
        if(_playerInSight && !_playerInRange) ChasePlayer();
        if(_playerInRange) AttackPlayer();

        RunningTimeout();
      }
    }

    private void ChasePlayer()
    {
      Cast();
      _agent.SetDestination(_player.position);
      Run();
    }

    private void AttackPlayer()
    {
        Vector3 _targetPos = new Vector3(_player.position.x, transform.position.y, _player.position.z);
        transform.LookAt(_targetPos);
        _attackTime -= Time.deltaTime;
        if(_attackTime < 0){
            Attack();
            _attackTime = _attackSpeed;
        }
        Vector3 _distanceToPlayer = transform.position - _targetPos;
        if (_distanceToPlayer.magnitude > 2.5f){
            _agent.SetDestination(_player.position);
            Run();
        }
        else{
            _agent.SetDestination(transform.position);
        }
    }

    private void Run(){
        _time = Time.time;
        enemyAnimator.SetBool("isWalking", true);
    }

    void RunningTimeout()
    {
        if(Mathf.Abs(_time - Time.time) > 0.1f) {
          enemyAnimator.SetBool("isWalking", false);
        }
    }

    private void Attack() {
      float _health = takeDamageEnemy.health;
      enemyAnimator.CrossFade("Attack", 0.2f);
      if (_health > 150){
        StartCoroutine(AttackWithDamage(20));
      }
      else if(_health < 150){
        StartCoroutine(AttackWithDamage(10));
      }
    }

    private void HeavySwing() {

    }

    void Cast(){
     if(_timeUntilMagic <= 0) {
       _timeUntilMagic = _magicTimeout;
       GameObject spell = Instantiate(magic, _castPoint.position, transform.rotation);
       Rigidbody rb = spell.GetComponent<Rigidbody>();
       rb.AddForce(transform.forward * (_projectileSpeed * 0.1f) * Time.deltaTime, ForceMode.VelocityChange);
     }
   }

    IEnumerator AttackWithDamage(float damage)
    {
        yield return new WaitForSeconds(1.5f);
        Vector3 _targetPos = new Vector3(_player.position.x, transform.position.y, _player.position.z);
        Vector3 _distanceToPlayer = transform.position - _targetPos;
        if (_distanceToPlayer.magnitude <= 2.5){
            playerObject.GetComponent<TakeDamagePlayer>().TakeDamage(damage);
        }
    }

}
