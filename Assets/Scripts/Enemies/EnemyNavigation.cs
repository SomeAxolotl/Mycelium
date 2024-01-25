using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavigation : MonoBehaviour
{
    public NavMeshAgent navMeshAgent;
    public bool playerSeen;
    private bool canChase = true;
    private bool startedPatrol = false;
    private float patrolRadius = 10f;
    private Vector3 previousPosition;
    private Collider[] playerColliders;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;
    private Transform player;
    [SerializeField] private float fieldOfView = 60f;
    [SerializeField] private float detectionRange = 25f;
    [SerializeField] private float detectionBuffer = 2f;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animator.SetBool("IsMoving", true);
        previousPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentVelocity = (transform.position - previousPosition) / Time.deltaTime;
        previousPosition = transform.position;
        float speed = currentVelocity.magnitude;

        playerColliders = Physics.OverlapSphere(transform.position, detectionRange, playerLayer);
        foreach (var playerCollider in playerColliders)
        {
            player = playerCollider.transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float dstToPlayer = Vector3.Distance(transform.position, player.position);
            var newRotation = Quaternion.LookRotation(dirToPlayer);
            
            if (Vector3.Angle(transform.forward, dirToPlayer) < fieldOfView && !Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), dirToPlayer, dstToPlayer, obstacleLayer) || 
                dstToPlayer <= (navMeshAgent.stoppingDistance + detectionBuffer) && !Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), dirToPlayer, dstToPlayer, obstacleLayer))
            {
                playerSeen = true;
                startedPatrol = false;
                if(canChase)
                {
                    StartCoroutine(ChasePlayer());
                }
                transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10f);
            }
            else
            {
                canChase = true;
                playerSeen = false;
            }
        }

        if (!startedPatrol && !playerSeen)
        {
            SetRandomDestination();
        }
        if (!playerSeen && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance + 1f || startedPatrol && speed < .25f)
        {
            startedPatrol = false;
        }
    }
    IEnumerator ChasePlayer()
    {
        canChase = false;
        navMeshAgent.SetDestination(player.position);
        yield return new WaitForSeconds(0.5f);
        canChase = true;
    }
    
    void SetRandomDestination()
    {
        Vector3 point;
        if (RandomPoint(transform.position, patrolRadius, out point) == true)
        {
            navMeshAgent.SetDestination(point);
            startedPatrol = true;
        }
    }

    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector2 randomPoint = Random.insideUnitCircle * patrolRadius;
        Vector3 worldPoint = transform.position + new Vector3(randomPoint.x, 0, randomPoint.y);
        NavMeshHit hit;

        if(NavMesh.SamplePosition(worldPoint, out hit, 10f, NavMesh.AllAreas))
        {
            Debug.Log("true" + hit.position);
            result = hit.position;
            return true;
        }
        else
        {
            Debug.Log("false" + hit.position);
            result = Vector3.zero;
            return false;
        }
    }
}
