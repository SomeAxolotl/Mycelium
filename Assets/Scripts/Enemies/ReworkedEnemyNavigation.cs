using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ReworkedEnemyNavigation : MonoBehaviour
{
    public bool playerSeen;
    [HideInInspector] public bool startedPatrol = false;
    private float patrolRadius = 20f;
    private float speed;
    private float rerouteTimer;
    private Vector3 origin;
    private Vector3 previousPosition;
    private Collider[] playerColliders;
    public LayerMask playerLayer;
    public LayerMask enviromentLayer;
    public LayerMask obstacleLayer;
    private Transform player;
    private Transform center;
    [SerializeField] private float fieldOfView = 60f;
    [SerializeField] private float forwardDetectionRange = 25f;
    [SerializeField] private float backwardsDetectionRange = 15f;
    public float moveSpeed = 3f;
    [SerializeField] private bool isFlyingEnemy;
    private float gravityForce = -10;
    private float maxRotationSpeed = 200f;
    Vector3 gravity;
    [HideInInspector] public Animator animator;
    Rigidbody rb;
    private List<Vector3> waypoints = new List<Vector3>();

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animator.SetBool("IsMoving", true);
        previousPosition = transform.position;
        center = transform.Find("CenterPoint");
        rb = GetComponent<Rigidbody>();
        gravity = new Vector3(0f, gravityForce, 0f);
        rb.useGravity = !isFlyingEnemy;
        origin = transform.position;
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
            Vector3 dirToPlayer = player.position - center.position;
            float dstToPlayer = Vector3.Distance(center.position, player.position);
            if (Vector3.Angle(transform.forward, dirToPlayer) < fieldOfView || dstToPlayer <= backwardsDetectionRange)
            {
                if (!Physics.Raycast(center.position, dirToPlayer, dstToPlayer, enviromentLayer))
                {
                    playerSeen = true;
                    startedPatrol = false;
                    waypoints.Clear();
                }
                else
                {
                    playerSeen = false;
                }
            }
        }
        
        if (!startedPatrol && !playerSeen)
        {
            if (!isFlyingEnemy)
            {
                SetRandomDestination();
            }
            else if (isFlyingEnemy)
            {
                SetRandomFlyingDestination();
            }
        }

        if(!isFlyingEnemy)
        {
            if (speed < 1f && !playerSeen)
            {
                rerouteTimer += Time.deltaTime;
            }
            else
            {
                rerouteTimer = 0f;
            }

            if (rerouteTimer > .5f)
            {
                waypoints.Clear();
                SetRandomDestination();
                rb.AddForce((-Vector3.forward * 3f) + (Vector3.up * 2f), ForceMode.Impulse);
                rerouteTimer = 0f;
            }
        }

        if(isFlyingEnemy)
        {
            if(!playerSeen)
            {
                rerouteTimer += Time.deltaTime;
            }
            else
            {
                rerouteTimer = 0f;
            }

            if (rerouteTimer > 4f)
            {
                waypoints.Clear();
                rb.velocity = Vector3.zero;
                SetRandomFlyingDestination();
                rerouteTimer = 0f;
            }
        }



    }
    void FixedUpdate()
    {
        if (!isFlyingEnemy)
        {
            rb.AddForce(gravity, ForceMode.Acceleration);
            GroundEnemyNavigation();
        }
        else if (isFlyingEnemy)
        {
            FlyingEnemyNavigation();
        }
    }
    void GroundEnemyNavigation()
    {
        if (startedPatrol && !playerSeen && waypoints.Count > 0)
        {
            Vector3 nextWaypoint = waypoints[0];
            float distanceToNextWaypoint = Vector3.Distance(transform.position, nextWaypoint);
            if (distanceToNextWaypoint <= 1f)
            {
                waypoints.RemoveAt(0);
                if (waypoints.Count == 0)
                {
                    startedPatrol = false;
                }
            }
            else
            {
                Vector3 moveDirection = ObstacleAvoidance(nextWaypoint - transform.position);
                rb.velocity = new Vector3((moveDirection * moveSpeed).x, rb.velocity.y, (moveDirection * moveSpeed).z);

                Quaternion desiredRotation = Quaternion.LookRotation(moveDirection);
                float desiredYRotation = desiredRotation.eulerAngles.y;
                Quaternion targetRotation = Quaternion.Euler(0f, desiredYRotation, 0f);
                float angleToTarget = Quaternion.Angle(transform.rotation, targetRotation);
                float maxAngleThisFrame = maxRotationSpeed * Time.fixedDeltaTime;
                float fractionOfTurn = Mathf.Min(maxAngleThisFrame / angleToTarget, 1f);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, fractionOfTurn);
            }
        }
        else if (playerSeen && player != null && gameObject.name.Contains("Mushy"))
        {
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(directionToPlayer.x, 0, directionToPlayer.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * maxRotationSpeed);
        }
    }
    void FlyingEnemyNavigation()
    {
        if (startedPatrol && !playerSeen && waypoints.Count > 0)
        {
            Vector3 targetPosition = waypoints[0];
            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
            if (distanceToTarget <= 1f)
            {
                waypoints.Clear();
                startedPatrol = false;
            }
            else
            {
                Vector3 moveDirection = ObstacleAvoidance(targetPosition - transform.position);
                rb.velocity = moveDirection * moveSpeed;

                Quaternion desiredRotation = Quaternion.LookRotation(moveDirection);
                float angleToTarget = Quaternion.Angle(transform.rotation, desiredRotation);
                float maxAngleThisFrame = maxRotationSpeed * 2f * Time.fixedDeltaTime;
                float fractionOfTurn = Mathf.Min(maxAngleThisFrame / angleToTarget, 1f);
                transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, fractionOfTurn);
            }
        }
    }
    Vector3 ObstacleAvoidance(Vector3 desiredDirection)
    {
        Vector3 moveDirection = desiredDirection.normalized;

        RaycastHit hit;
        if (Physics.Raycast(center.position, transform.forward, out hit, 3f, obstacleLayer) && speed <= .5f)
        {
            waypoints.Clear();
            if (!isFlyingEnemy)
            {
                SetRandomDestination();
                rb.AddForce((-Vector3.forward * 3f) + (Vector3.up * 2f), ForceMode.Impulse);
            }
            else if (isFlyingEnemy)
            {
                SetRandomFlyingDestination();
            }
            rerouteTimer = 0f;
        }
        else if (Physics.Raycast(center.position, transform.forward, out hit, 3f, obstacleLayer) && speed > .5f)
        {
            Vector3 avoidanceDirection = Vector3.Cross(Vector3.up, hit.normal);
            moveDirection += avoidanceDirection * 1f;
        }

        return moveDirection.normalized;
    }
    public void SetRandomDestination()
    {
        Vector3 point;
        if (RandomPoint(transform.position, patrolRadius, out point))
        {
            NavMeshPath path = new NavMeshPath();
            if (NavMesh.CalculatePath(transform.position, point, NavMesh.AllAreas, path))
            {
                waypoints.Clear();

                for (int i = 0; i < path.corners.Length; i++)
                {
                    waypoints.Add(path.corners[i]);
                }
            }
            startedPatrol = true;
        }
    }
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector2 randomPoint = Random.insideUnitCircle * patrolRadius;
        Vector3 worldPoint = transform.position + new Vector3(randomPoint.x, 0, randomPoint.y);
        NavMeshHit hit;

        if (NavMesh.SamplePosition(worldPoint, out hit, patrolRadius, NavMesh.AllAreas))
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
    public void SetRandomFlyingDestination()
    {
        Debug.Log("ATTEMPTING");
        Vector3 target;
        if (RandomFlyingPoint(out target))
        {
            waypoints.Clear();
            waypoints.Add(target);
            startedPatrol = true;
            //Debug.DrawLine(transform.position, target, UnityEngine.Color.green, 2f);
            //Debug.Log("target: " + target);
        }
    }
    bool RandomFlyingPoint(out Vector3 result)
    {
        Vector2 randomPoint = Random.insideUnitCircle * patrolRadius;
        Vector3 worldPoint = origin + new Vector3(randomPoint.x, 0, randomPoint.y);

        if(Physics.Raycast(worldPoint, Vector3.forward, 3f, enviromentLayer) || 
           Physics.Raycast(worldPoint, Vector3.back, 3f, enviromentLayer) || 
           Physics.Raycast(worldPoint, Vector3.right, 3f, enviromentLayer) || 
           Physics.Raycast(worldPoint, Vector3.left, 3f, enviromentLayer))
        {
            result = Vector3.zero;
            return false;
        }
        else
        {
            result = worldPoint;
            return true;
        }
    }
}
