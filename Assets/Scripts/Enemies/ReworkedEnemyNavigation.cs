using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class ReworkedEnemyNavigation : MonoBehaviour
{
    [HideInInspector] public bool playerSeen;
    [HideInInspector] public bool startedPatrol = false;
    private float patrolRadius = 20f;
    private float speed;
    private float rerouteTimer;
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
    private float gravityForce = -10;
    Vector3 gravity;
    [HideInInspector] public Animator animator;
    //public bool undergrowthSpeed;
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
            SetRandomDestination();
        }

        if (speed < 1f && !playerSeen)
        {
            rerouteTimer += Time.deltaTime;
        }
        else
        {
            rerouteTimer = 0;
        }

        if (rerouteTimer > .5f)
        {
            waypoints.Clear();
            SetRandomDestination();
            rerouteTimer = 0f;
            Debug.Log("patrol restart via timer! " + gameObject.name);
        }

        //Debug.DrawRay(center.position, transform.forward * 2f, Color.red);
    }
    private void FixedUpdate()
    {
        rb.AddForce(gravity, ForceMode.Acceleration);

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
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8f);
            }
        }
    }
    Vector3 ObstacleAvoidance(Vector3 desiredDirection)
    {
        Vector3 moveDirection = desiredDirection.normalized;

        RaycastHit hit;
        if (Physics.Raycast(center.position, transform.forward, out hit, 2f, obstacleLayer) && speed <= .5f)
        {
            waypoints.Clear();
            SetRandomDestination();
            rerouteTimer = 0f;
            Debug.Log("patrol restart via raycast! " + gameObject.name);
        }
        else if (Physics.Raycast(center.position, transform.forward, out hit, 2f, obstacleLayer) && speed > .5f)
        {
            Vector3 avoidanceDirection = Vector3.Cross(Vector3.up, hit.normal);
            moveDirection += avoidanceDirection * 1f;
        }

        return moveDirection.normalized;
    }
    void SetRandomDestination()
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
}
