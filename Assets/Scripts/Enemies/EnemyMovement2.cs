using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement2 : MonoBehaviour
{
    [HideInInspector] public NavMeshAgent navMeshAgent;
    public bool playerSeen;
    public bool attacking = false;
    private bool startedPatrol = false;
    private float patrolRadius = 10f;
    [HideInInspector] public float speed;
    private float rerouteTimer;
    [HideInInspector] public float attackStopdistance;
    private Vector3 previousPosition;
    private Collider[] playerColliders;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;
    private Transform player;
    [SerializeField] private float fieldOfView = 60f;
    [SerializeField] private float detectionRange = 25f;
    [SerializeField] private float detectionBuffer = 12f;
    public bool undergrowthSpeed;

    // Start is called before the first frame update
    void Start()
    {
        //transform.rotation = Quaternion.Euler(46, 0, 0);
        navMeshAgent = GetComponent<NavMeshAgent>();
        previousPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(46, 0, 0);
        Vector3 currentVelocity = (transform.position - previousPosition) / Time.deltaTime;
        previousPosition = transform.position;
        speed = currentVelocity.magnitude;

        playerColliders = Physics.OverlapSphere(transform.position, detectionRange, playerLayer);
        foreach (var playerCollider in playerColliders)
        {
            player = playerCollider.transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float dstToPlayer = Vector3.Distance(transform.position, player.position);
            var newRotation = Quaternion.LookRotation(dirToPlayer);

            if (Vector3.Angle(transform.forward, dirToPlayer) < fieldOfView && !Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), dirToPlayer, dstToPlayer, obstacleLayer) ||
                dstToPlayer <= (navMeshAgent.stoppingDistance + detectionBuffer))
            {
                playerSeen = true;
                navMeshAgent.SetDestination(player.position);
                startedPatrol = false;
                if (!attacking)
                {
                    newRotation = Quaternion.LookRotation(dirToPlayer);
                    newRotation.eulerAngles = new Vector3(46f, newRotation.eulerAngles.y, newRotation.eulerAngles.z);
                    transform.rotation = newRotation;
                }
                else if (attacking)
                {
                    transform.rotation = transform.rotation;
                }
            }
            else
            {
                
                playerSeen = false;
            }
        }

        if (speed < .25f && !playerSeen)
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

        if (NavMesh.SamplePosition(worldPoint, out hit, 10f, NavMesh.AllAreas))
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
