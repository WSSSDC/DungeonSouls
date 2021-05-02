using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent _agent;
    public LayerMask _whatIsGround, _whatIsPlayer;
    public float _sightRange, _attackRange, _moveSpeed, _wanderRange, _attackSpeed, _blockTendancy, _projectileSpeed, _magicTimeout = 5000f, distanceThreshold;
    public GameObject magic;
    public Transform _castPoint;
    private Animator enemyAnimator;
    private bool _wanderLocationFound = false, _playerInSight = false, _playerInRange = false;
    private Vector3 _wanderLocation;
    private float _attackTime;
    private Transform _player;
    private float _time;
    private float _timeUntilMagic;
    private GameObject playerObject;
    private TakeDamageEnemy takeDamageEnemy;

    void Start()
    {
        _timeUntilMagic = _magicTimeout;
        _attackTime = _attackSpeed;
        _player = GameObject.Find("Character").transform;
        enemyAnimator = GetComponent<Animator>();
        takeDamageEnemy = GetComponent<TakeDamageEnemy>();
        _time = Time.time;
    }


    void Update()
    {
      if(!takeDamageEnemy.dead) {
        _timeUntilMagic -= Time.deltaTime;

        _playerInSight = Physics.CheckSphere(transform.position, _sightRange, _whatIsPlayer);
        _playerInRange = Physics.CheckSphere(transform.position, _attackRange, _whatIsPlayer);

        if(!_playerInSight && !_playerInRange)Wander();
        if(_playerInSight && !_playerInRange)ChasePlayer();
        if(_playerInSight && _playerInRange)AttackPlayer();

        RunningTimeout();
      }
    }


    private void Wander()
    {
        if(_wanderLocationFound)
        {
            _agent.SetDestination(_wanderLocation);
            Run();
        }else
        {
            SearchWanderLocation();
        }
        Vector3 _distanceToWanderLocation = transform.position - _wanderLocation;
        if(_distanceToWanderLocation.magnitude < 1f)_wanderLocationFound = false;
    }

    private void SearchWanderLocation()
    {
        float randomZ = Random.Range(-_wanderRange, _wanderRange);
        float randomX = Random.Range(-_wanderRange, _wanderRange);
        _wanderLocation = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if(Physics.Raycast(_wanderLocation, -transform.up, 2f, _whatIsGround))_wanderLocationFound = true;
    }

    private void ChasePlayer()
    {
        _agent.SetDestination(_player.position);
        CastMagic();
        Run();
    }

    private void AttackPlayer()
    {
        Vector3 _targetPos = new Vector3(_player.position.x, transform.position.y, _player.position.z);
        transform.LookAt(_targetPos);
        _attackTime -= Time.deltaTime;
        if(_attackTime < 0){
            HeavySwing();
            _attackTime = _attackSpeed;
        }
        Vector3 _distanceToPlayer = transform.position - _targetPos;
        if (_distanceToPlayer.magnitude > 5f){
            CastMagic();
            _agent.SetDestination(_player.position);
            Run();
        }
        else{
            _agent.SetDestination(transform.position);
        }
    }

    private void Run(){
      _time = Time.time;
      enemyAnimator.SetBool("isRunning", true);
    }

    private void HeavySwing(){
        enemyAnimator.CrossFade("Attack", 0.2f);
        StartCoroutine(Attack());
    }

    void RunningTimeout()
    {
        if(Mathf.Abs(_time - Time.time) > 0.1f) {
          enemyAnimator.SetBool("isRunning", false);
        }
    }

     void CastMagic(){
      if(_timeUntilMagic <= 0) {
        _timeUntilMagic = _magicTimeout;
        GameObject spell = Instantiate(magic, _castPoint.position, transform.rotation);
        Rigidbody rb = spell.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * (_projectileSpeed * 0.1f) * Time.deltaTime, ForceMode.VelocityChange);
      }
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(1.2f);
        Vector3 _targetPos = new Vector3(_player.position.x, transform.position.y, _player.position.z);
        Vector3 _distanceToPlayer = transform.position - _targetPos;
        if (_distanceToPlayer.magnitude <= distanceThreshold + 1){
            playerObject.GetComponent<TakeDamagePlayer>().TakeDamage(20);
        }
    }
}
