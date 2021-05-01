using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent _agent;
    public LayerMask _whatIsGround, _whatIsPlayer;
    public float _sightRange, _attackRange, _moveSpeed, _wanderRange, _attackSpeed;
    public GameObject magic;
    public Animator enemyAnimator;
    private bool _wanderLocationFound = false, _playerInSight = false, _playerInRange = false;
    private Vector3 _wanderLocation;
    private float _attackTime;
    private Transform _player, _castPoint;
    
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

    private void Awake() 
    {
        _agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
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
        if(!TooCloseToPlayer(transform, _player)){
            _agent.SetDestination(_player.position);
            Run(); 
        }
        else{
            Vector3 _dest = new Vector3(transform.position.x - 10f, transform.position.y, transform.position.z-10f);
            _agent.SetDestination(_dest);
            Run();
        }
    }
    private bool TooCloseToPlayer(Transform enemy, Transform player){
        return ((enemy.position - player.position).magnitude < 10f);
    }

    private void AttackPlayer()
    {
        Vector3 _targetPos = new Vector3(_player.position.x, transform.position.y, _player.position.z);
        transform.LookAt(_targetPos);
        _attackTime -= Time.deltaTime;
        if(_attackTime < 0){
            CastMagic();
            _attackTime = _attackSpeed;
        }
        Vector3 _distanceToPlayer = transform.position - _targetPos;
        if (_distanceToPlayer.magnitude < 10f){
            Vector3 _dest = new Vector3(transform.position.x - 10f, transform.position.y, transform.position.z-10f);
            _agent.SetDestination(_dest);
            Run();
        }
        else{
            _agent.SetDestination(transform.position);
        }
    }

    private void Run(){
        if(!enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("run"))
        enemyAnimator.Play("run");
    }

    private void CastMagic(){
        //TODO: Set up magic
    }
}
