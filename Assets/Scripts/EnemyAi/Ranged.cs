using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ranged : MonoBehaviour
{
    public UnityEngine.AI.NavMeshAgent _agent;
    public LayerMask _whatIsGround, _whatIsPlayer;
    public float _sightRange, _moveSpeed, _wanderRange, _attackSpeed, _projectileSpeed;
    public GameObject arrow;
    public Transform _shootPoint;
    public Animator enemyAnimator;
    private bool _wanderLocationFound = false, _playerInSight = false;
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

        if(!_playerInSight)Wander();
        if(_playerInSight)AttackPlayer();
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


    private void AttackPlayer()
    {
        //Vector3 _targetPos = new Vector3(_player.position.x, _player.position.y, _player.position.z);
        transform.LookAt(_player.position);
        _attackTime -= Time.deltaTime;
        if(_attackTime < 0){
            ShootArrow();
            _attackTime = _attackSpeed;
        }
    }

    private void Run(){
        if(!enemyAnimator.GetCurrentAnimatorStateInfo(0).IsName("run"))
        enemyAnimator.Play("run");
    }

    private void ShootArrow(){
        GameObject Arr = Instantiate(arrow, _shootPoint.position, _shootPoint.rotation);
        Rigidbody rb = Arr.GetComponent<Rigidbody>();
        rb.AddForce(transform.forward * _projectileSpeed * Time.deltaTime, ForceMode.VelocityChange);
    }
}
