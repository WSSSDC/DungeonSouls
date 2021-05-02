using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent _agent;
    public LayerMask _whatIsGround, _whatIsPlayer;
    public float _sightRange, _attackRange, _moveSpeed, _wanderRange, _attackSpeed, _blockTendancy, _damage, distanceThreshold = 4f;
    private Animator enemyAnimator;
    private bool _wanderLocationFound = false, _playerInSight = false, _playerInRange = false;
    private Vector3 _wanderLocation;
    private float _attackTime;
    private Transform _player;
    private bool isRunning;
    private float _time;
    private GameObject playerObject;
    private bool isAttacking = false;

    void Start()
    {
        _attackTime = _attackSpeed;
        playerObject = GameObject.Find("Character_Hero_Knight_Male");
        _player = GameObject.Find("Character").transform;
        enemyAnimator = GetComponent<Animator>();
        _time = Time.time;
    }


    void Update()
    {
        if(!GetComponent<TakeDamageEnemy>().dead) {
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
        Run();
    }

    private void AttackPlayer()
    {
        Vector3 _targetPos = new Vector3(_player.position.x, transform.position.y, _player.position.z);
        transform.LookAt(_targetPos);
        _attackTime -= Time.deltaTime;
        if(_attackTime < 0){
            LightAttack();
            _attackTime = _attackSpeed;
        }
        Vector3 _distanceToPlayer = transform.position - _targetPos;
        if (_distanceToPlayer.magnitude > distanceThreshold){
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

    private void LightAttack(){
        //TODO: Set up light attack with Connor
        enemyAnimator.CrossFade("Attack", 0.2f);
        StartCoroutine(Attack());
    }

    void RunningTimeout()
    {
        if(Mathf.Abs(_time - Time.time) > 0.1f) {
          enemyAnimator.SetBool("isRunning", false);
        }
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(1f);
        isAttacking = false;
        Vector3 _targetPos = new Vector3(_player.position.x, transform.position.y, _player.position.z);
        Vector3 _distanceToPlayer = transform.position - _targetPos;
        if (_distanceToPlayer.magnitude <= distanceThreshold + 1){
            playerObject.GetComponent<TakeDamagePlayer>().TakeDamage(_damage);
        }
    }
}
