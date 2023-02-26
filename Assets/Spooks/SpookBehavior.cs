using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SpookBehavior : MonoBehaviour
{
    private GameController gameController;
    private Animator animator;
    [SerializeField]
    private NavMeshAgent navMeshAgent;
    [SerializeField]
    private bool isChasing;
    private Transform Player;
    private Vector3 home;
    private Vector3 destination;
    private float distanceToTarget;
    private float patrolRadius = 10f;
    private List<Vector3> patrolPoints = new List<Vector3>();
    private int currentPatrolPointIndex;
    private float defaultSpeed;

    public GameObject Face;
    public Shader Specular;
    public Shader Toon;
    public bool IsChasing
    {
        get { return isChasing; }
        set { 
            isChasing = value;            
            animator.SetBool("Chasing", value);

            if (value)
            {
                navMeshAgent.speed = navMeshAgent.speed * 2;
                Face.GetComponent<Renderer>().material.shader = Toon;
            }
            else
            {
                navMeshAgent.speed = defaultSpeed;
                Face.GetComponent<Renderer>().material.shader = Specular;
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameController = GameObject.Find("GameController").GetComponent<GameController>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        home = GetComponent<Transform>().position;

        switch (GameManager.Instance.Difficulty)
        {
            case GameManager.DifficultyLevel.Easy:
                patrolRadius = 15;
                navMeshAgent.speed = 3;
                break;
            case GameManager.DifficultyLevel.Normal:
                patrolRadius = 20;
                navMeshAgent.speed = 5;
                break;
            case GameManager.DifficultyLevel.Hard:
                patrolRadius = 25;
                navMeshAgent.speed = 8;
                break;
            default:
                break;

        }
        defaultSpeed = navMeshAgent.speed;

        // Generate a list of patrol points around the home position
        for (int i = 0; i < 10; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * (patrolRadius*2);
            randomDirection += home;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, patrolRadius, 1);
            patrolPoints.Add(hit.position);
        }
        Patrol();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameManager.Instance.IsGamePaused && !GameManager.Instance.IsDebuging)
        {
            if(navMeshAgent.isStopped)
                navMeshAgent.isStopped= false;

            //Check if player is close
            distanceToTarget = Vector3.Distance(transform.position, Player.position);

            if (distanceToTarget < patrolRadius || isChasing)
                Chase();
            else if (navMeshAgent.remainingDistance < 1f && !IsChasing)
            {
                Patrol();
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
                destination = Player.position;
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
            destination = Player.position;
            navMeshAgent.SetDestination(destination);
            IsChasing = true;
            navMeshAgent.isStopped = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "PlayerCameraRoot & FlashLight")
        {
            isChasing = false;
            navMeshAgent.SetDestination(home);
        }
        else if (other.tag == "Player")
            other.transform.position = GameObject.FindGameObjectWithTag("Respawn").transform.position;
    }

    private void Patrol()
    {
        currentPatrolPointIndex = Random.Range(0, patrolPoints.Count);
        navMeshAgent.SetDestination(patrolPoints[currentPatrolPointIndex]);

        // Reset the animator
        animator.SetBool("Chasing", false);
        IsChasing = false;
        navMeshAgent.isStopped = false;
    }
}
