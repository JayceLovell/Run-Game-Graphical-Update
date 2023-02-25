using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpookBehavior : MonoBehaviour
{
    private GameController gameController;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    [SerializeField]
    private bool isChasing;
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Vector3 home;
    [SerializeField]
    private Vector3 destination;
    [SerializeField]
    private float distanceToTarget;
    [SerializeField]
    private float patrolRadius = 10f;
    private GameObject[] PatrolPoints;

    public bool IsChasing
    {
        get { return isChasing; }
        set { 
            isChasing = value;
            animator.SetBool("Chasing", value);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        home = GetComponent<Transform>().position;
        PatrolPoints = GameObject.FindGameObjectsWithTag("EnemyPosition");

        switch (GameManager.Instance.Difficulty)
        {
            case GameManager.DifficultyLevel.Easy:
                patrolRadius = 10;
                navMeshAgent.speed = 3;
                break;
            case GameManager.DifficultyLevel.Normal:
                patrolRadius = 15;
                navMeshAgent.speed = 5;
                break;
            case GameManager.DifficultyLevel.Hard:
                patrolRadius = 20;
                navMeshAgent.speed = 8;
                break;
            default:
                break;

        }
        Patrol();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.IsGamePaused)
        {
            distanceToTarget = Vector3.Distance(transform.position, target.position);
            if (distanceToTarget < patrolRadius || isChasing)
                Chase();
            else if (navMeshAgent.remainingDistance < 0.1f && !IsChasing)
            {
                Patrol();
                IsChasing = false;
            }
        }
        else
            navMeshAgent.isStopped = true;
    }
    private void Chase()
    {
        if (IsChasing)
        {
            if (distanceToTarget < 50.0f)
            {
                destination = target.position;
                navMeshAgent.SetDestination(destination);
            }
            else
            {
                navMeshAgent.SetDestination(home);
                IsChasing = false;
            }

        }
        else
        {
            destination = target.position;
            navMeshAgent.SetDestination(destination);
            IsChasing = true;
            navMeshAgent.isStopped = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name== "PlayerCameraRoot & FlashLight")
        {
            navMeshAgent.SetDestination(home);
        }
    }

    private void Patrol()
    {
        Vector3 randomPositon = PatrolPoints[Random.Range(0, PatrolPoints.Length)].transform.position;

        navMeshAgent.SetDestination(randomPositon);

        // Reset the animator
        animator.SetBool("Chasing", false);
        IsChasing = false;
        navMeshAgent.isStopped = false;
    }
}
