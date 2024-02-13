using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavigation : MonoBehaviour
{
    [HideInInspector] public NavMeshAgent navMeshAgent;
    public bool playerSeen;
    public bool attacking = false;
    public bool startedPatrol = false;
    private float patrolRadius = 10f;
    [HideInInspector] public float speed;
    private float rerouteTimer;
    private Vector3 previousPosition;
    private Collider[] playerColliders;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;
    private Transform player;
    private Transform center;
    [SerializeField] private float fieldOfView = 60f;
    [SerializeField] private float forwardDetectionRange = 25f;
    [SerializeField] private float backwardsDetectionRange = 15f;
    [HideInInspector] public Animator animator;
    public bool undergrowthSpeed;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        animator.SetBool("IsMoving", true);
        previousPosition = transform.position;
        center = transform.Find("CenterPoint");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentVelocity = (transform.position - previousPosition) / Time.deltaTime;
        previousPosition = transform.position;
        speed = currentVelocity.magnitude;

        playerColliders = Physics.OverlapSphere(transform.position, forwardDetectionRange, playerLayer);
        foreach (var playerCollider in playerColliders)
        {
            player = playerCollider.transform.Find("CenterPoint");
            Vector3 dirToPlayer = (player.position - center.position).normalized;
            float dstToPlayer = Vector3.Distance(center.position, player.position);
            var newRotation = Quaternion.LookRotation(dirToPlayer);
            if (Vector3.Angle(transform.forward, dirToPlayer) < fieldOfView || dstToPlayer <= backwardsDetectionRange)
            {
                if(!Physics.Raycast(center.position, dirToPlayer, dstToPlayer, obstacleLayer))
                {
                    playerSeen = true;
                    startedPatrol = false;
                }
                else
                {
                    playerSeen = false;
                }
            }
        }

        if(speed < .25f && !playerSeen)
        {
            rerouteTimer += Time.deltaTime;
        }
        else
        {
            rerouteTimer = 0;
        }

        if (!startedPatrol && !playerSeen)
        {
            SetRandomDestination();
        }

        if (!playerSeen && navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance + .5f || rerouteTimer > 1.5f)
        {
            startedPatrol = false;
        }
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
            result = hit.position;
            return true;
        }
        else
        {
            result = Vector3.zero;
            return false;
        }
    }
}
