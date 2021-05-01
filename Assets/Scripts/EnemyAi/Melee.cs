using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent _agent;
    public LayerMask _whatIsGround, _whatIsPlayer;
    public float _sightRange, _attackRange, _moveSpeed, _wanderRange, _attackSpeed, _blockTendancy;
    public Animator enemyAnimator;
    private bool _wanderLocationFound = false, _playerInSight = false, _playerInRange = false;
    private Vector3 _wanderLocation;
    private float _attackTime;
    private Transform _player;
    
        void Start()
    {
        _attackTime = _attackSpeed;
        _player = GameObject.Find("Character").transform;
    }

    
    void Update()
    {
        _playerInSight = Physics.CheckSphere(transform.position, _sightRange, _whatIsPlayer);
        _playerInRange = Physics.CheckSphere(transform.position, _attackRange, _whatIsPlayer);

        if(!_playerInSight && !_playerInRange)Wander();
        if(_playerInSight && !_playerInRange)ChasePlayer();
        if(_playerInSight && _playerInRange)AttackPlayer();
    }


    private void Wander()
    {
        if(_wanderLocationFound)
        {
            _agent.SetDestination(_wanderLocation);
            //Run();
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
        //Run();
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
        if (_distanceToPlayer.magnitude > 2f){
            _agent.SetDestination(_player.position);
            //Run();
        }
        else{
            _agent.SetDestination(transform.position);
        }
    }

    private void Run(){
        if(!enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("run"))
        enemyAnimator.Play("run");
    }

    private void LightAttack(){
        //TODO: Set up light attack with Connor
    }
}
