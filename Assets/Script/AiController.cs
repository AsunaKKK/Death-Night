using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiController : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public float startWaitTime = 4;
    public float timeToRotate = 2;
    public float speedWalk = 6;
    public float speedRun = 9;

    public float viewRadius = 15;
    public float viewAngle = 90;
    public LayerMask playerMask;
    public LayerMask obstacleMask;
    public float meshResolution = 1.0f;
    public int edgeIterations = 4;
    public float edgeDistance = 0.5f;


    public Transform[] waypoints;
    int CurrentWaypointIndex;

    Vector3 playerLastPosition = Vector3.zero;
    Vector3 PlayerPosition;

    float WaitTime;
    float TimeToRotate;
    bool playerInRange;
    bool PlayerNear;
    bool IsPatrol;
    bool CaughtPlayer;


    public float health = 100f;
    public float EnemyDmg = 10f;

    [SerializeField] private Animator animator;

    public AudioSource atks;
    public AudioSource die;


    void Start()
    {
        animator = GetComponent<Animator>();
        PlayerPosition = Vector3.zero;
        IsPatrol = true;
        CaughtPlayer = false;
        playerInRange = false;
        PlayerNear = false;
        WaitTime = startWaitTime;
        TimeToRotate = timeToRotate;

        CurrentWaypointIndex = 0;
        navMeshAgent = GetComponent<NavMeshAgent>();

        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speedWalk;
        navMeshAgent.SetDestination(waypoints[CurrentWaypointIndex].position);
    }

    private void Update()
    {
        EnviromentView();

        if (!IsPatrol)
        {
            Chasing();
            animator.SetBool("run", true);
        }
        else
        {
            Patroling();
            animator.SetBool("walk", true);
        }
    }
    public void takeDamage(float dmg)
    {

        if (health <= 0f)
        {
            StartCoroutine("Dies");
        }

        health -= dmg;

    }

    IEnumerator Dies()
    {
        animator.SetTrigger("die");
        die.Play();
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
        EnemyDmg = 0;
        speedRun = 0;
        speedWalk = 0;
        animator.SetBool("walk", false);
        animator.SetBool("run", false);
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
    /*void Die()
    {
        animator.SetTrigger("die");
        Destroy(gameObject, 2f);
    }*/
    private void Chasing()
    {
        PlayerNear = false;
        playerLastPosition = Vector3.zero;
        atks.Play();

        if (!CaughtPlayer)
        {
            Move(speedRun);
            navMeshAgent.SetDestination(PlayerPosition);
        }
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            if (WaitTime <= 0 && !CaughtPlayer && Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 6f)
            {
                IsPatrol = true;
                PlayerNear = false;
                Move(speedWalk);
                TimeToRotate = timeToRotate;
                WaitTime = startWaitTime;
                navMeshAgent.SetDestination(waypoints[CurrentWaypointIndex].position);
            }
            else
            {
                if (Vector3.Distance(transform.position, GameObject.FindGameObjectWithTag("Player").transform.position) >= 2.5f)
                    Stop();
                WaitTime -= Time.deltaTime;
            }
        }
    }

    private void Patroling()
    {
        if (PlayerNear)
        {
            if (TimeToRotate <= 0)
            {
                Move(speedWalk);
                LookingPlayer(playerLastPosition);
            }
            else
            {
                Stop();
                TimeToRotate -= Time.deltaTime;
            }
        }
        else
        {
            PlayerNear = false;
            playerLastPosition = Vector3.zero;
            navMeshAgent.SetDestination(waypoints[CurrentWaypointIndex].position);
            if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
            {
                if (WaitTime <= 0)
                {
                    NextPoint();
                    Move(speedWalk);
                    WaitTime = startWaitTime;
                }
                else
                {
                    Stop();
                    WaitTime -= Time.deltaTime;
                }
            }
        }
    }
    public void NextPoint()
    {
        CurrentWaypointIndex = (CurrentWaypointIndex + 1) % waypoints.Length;
        navMeshAgent.SetDestination(waypoints[CurrentWaypointIndex].position);
    }

    void Stop()
    {
        navMeshAgent.isStopped = true;
        navMeshAgent.speed = 0;
        animator.SetBool("walk", false);
    }

    void Move(float speed)
    {
        navMeshAgent.isStopped = false;
        navMeshAgent.speed = speed;
    }

    void CaughtPlayers()
    {
        CaughtPlayer = true;
    }

    void LookingPlayer(Vector3 player)
    {
        navMeshAgent.SetDestination(player);
        if (Vector3.Distance(transform.position, player) <= 0.3)
        {
            if (WaitTime <= 0)
            {
                PlayerNear = false;
                Move(speedWalk);
                navMeshAgent.SetDestination(waypoints[CurrentWaypointIndex].position);
                WaitTime = startWaitTime;
                TimeToRotate = timeToRotate;
            }
            else
            {
                Stop();
                WaitTime -= Time.deltaTime;
            }
        }
    }

    void EnviromentView()
    {
        Collider[] playerInRanges = Physics.OverlapSphere(transform.position, viewRadius, playerMask);
        for (int i = 0; i < playerInRanges.Length; i++)
        {
            Transform player = playerInRanges[i].transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dirToPlayer) < viewAngle / 2)
            {
                float dstToPlayer = Vector3.Distance(transform.position, player.position);
                if (!Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleMask))
                {
                    playerInRange = true;
                    IsPatrol = false;
                }
                else
                {

                    playerInRange = false;
                }
            }
            if (Vector3.Distance(transform.position, player.position) > viewRadius)
            {

                playerInRange = false;
            }
            if (playerInRange)
            {
                PlayerPosition = player.transform.position;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerController pp = other.GetComponent<PlayerController>();
            if (pp != null)
            {
                pp.enemyTakeDamage(EnemyDmg);
            }
            animator.Play("atack2");
            animator.SetBool("walk", false);
        }
    }
}
