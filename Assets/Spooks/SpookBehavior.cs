using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpookBehavior : MonoBehaviour
{
    private GameController gameController;
    private Animator animator;
    private NavMeshAgent navMeshAgent;
    private bool isChasing;
    private Transform target;
    private Vector3 home;
    private Vector3 destination;
    private float patrolRadius = 10f;
    private float chasingRange;

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
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        home = GetComponent<Transform>().position;

        switch (gameController.GameManager.Difficulty)
        {
            case GameManager.DifficultyLevel.Easy:
                patrolRadius = 5;
                navMeshAgent.speed = 3;
                break;
            case GameManager.DifficultyLevel.Normal:
                patrolRadius = 10;
                navMeshAgent.speed = 5;
                break;
            case GameManager.DifficultyLevel.Hard:
                patrolRadius = 15;
                navMeshAgent.speed = 8;
                break;
            default:
                break;

        }
        SetRandomDestination();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameController.GameManager.IsGamePaused)
        {
            if (navMeshAgent.remainingDistance < 0.1f)
            {
                SetRandomDestination();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name== "PlayerCameraRoot & FlashLight")
        {
            //Vector3 fleeDirection = (transform.position - target.position).normalized;
            //Vector3 fleeDestination = transform.position + fleeDirection * 10f;
            navMeshAgent.SetDestination(home);
        }
    }

    private void SetRandomDestination()
    {
        float randomAngle = Random.Range(0f, 360f);
        float randomX = home.x + patrolRadius * Mathf.Sin(randomAngle * Mathf.Deg2Rad);
        float randomZ = home.z + patrolRadius * Mathf.Cos(randomAngle * Mathf.Deg2Rad);
        destination = new Vector3(randomX, home.y, randomZ);
        navMeshAgent.SetDestination(destination);
    }
}
