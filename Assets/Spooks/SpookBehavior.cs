using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpookBehavior : MonoBehaviour
{
    private GameController _gameController;
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private bool isChasing;
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Vector3 home;

    public bool IsChasing
    {
        get { return isChasing; }
        set { 
            isChasing = value;
            _animator.SetBool("Chasing", value);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        //_gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        _animator = GetComponent<Animator>();
        _navMeshAgent = GetComponent<NavMeshAgent>();
        //_navMeshAgent.SetDestination(target.position);
        home = GetComponent<Transform>().position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name== "PlayerCameraRoot & FlashLight")
        {
            //Vector3 fleeDirection = (transform.position - target.position).normalized;
            //Vector3 fleeDestination = transform.position + fleeDirection * 10f;
            _navMeshAgent.SetDestination(home);
        }
    }
}
