using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class IdleWalking : MonoBehaviour
{
    private float wanderRadius = 30f;
    private List<Vector3> waypoints = new List<Vector3>();
    private bool startedWander;
    private bool startedFirstWander;
    private float speed;
    private float rerouteTimer;
    private Vector3 previousPosition;
    public LayerMask environmentLayer;
    private float gravityForce;
    public float moveSpeed = 2f;
    Vector3 gravity;
    private Rigidbody rb;
    private Transform center;
    private Animator animator;
    // Start is called before the first frame update
    void OnEnable()
    {
        startedWander = true;
        startedFirstWander = false;
        animator = GetComponent<Animator>();
        previousPosition = transform.position;
        rb = GetComponent<Rigidbody>();
        center = transform.Find("CenterPoint");
        gravityForce = 0f;
        gravity = new Vector3(0f, gravityForce, 0f);
        Invoke("BeginFirstWander", 3f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentVelocity = (transform.position - previousPosition) / Time.deltaTime;
        previousPosition = transform.position;
        speed = currentVelocity.magnitude;

        if (!startedWander)
        {
            SetRandomDestination();
        }

        if (speed < .5f)
        {
            rerouteTimer += Time.deltaTime;
        }
        else
        {
            rerouteTimer = 0;
        }

        if (rerouteTimer > .5f && startedFirstWander)
        {
            waypoints.Clear();
            SetRandomDestination();
            rerouteTimer = 0f;
            Debug.Log("wander restart via timer! " + gameObject.name);
        }
    }
    private void FixedUpdate()
    {
        rb.AddForce(gravity, ForceMode.Acceleration);

        if (startedWander && waypoints.Count > 0)
        {
            Vector3 nextWaypoint = waypoints[0];
            float distanceToNextWaypoint = Vector3.Distance(transform.position, nextWaypoint);
            if (distanceToNextWaypoint <= 1f)
            {
                waypoints.RemoveAt(0);
                if (waypoints.Count == 0)
                {
                    startedWander = false;
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
        if (Physics.Raycast(center.position, transform.forward, out hit, 2f, environmentLayer) && speed <= .5f)
        {
            waypoints.Clear();
            SetRandomDestination();
            rerouteTimer = 0f;
            Debug.Log("wander restart via raycast! " + gameObject.name);
        }
        else if (Physics.Raycast(center.position, transform.forward, out hit, 2f, environmentLayer) && speed > .5f)
        {
            Vector3 avoidanceDirection = Vector3.Cross(Vector3.up, hit.normal);
            moveDirection += avoidanceDirection * 1f;
        }

        return moveDirection.normalized;
    }
    void SetRandomDestination()
    {
        Vector3 point;
        if (RandomPoint(transform.position, wanderRadius, out point))
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
            startedWander = true;
        }
    }
    bool RandomPoint(Vector3 center, float range, out Vector3 result)
    {
        Vector2 randomPoint = Random.insideUnitCircle * wanderRadius;
        Vector3 worldPoint = transform.position + new Vector3(randomPoint.x, 0, randomPoint.y);
        NavMeshHit hit;

        if (NavMesh.SamplePosition(worldPoint, out hit, wanderRadius, NavMesh.AllAreas))
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
    private void BeginFirstWander()
    {
        gravityForce = -10f;
        animator.SetBool("Walk", true);
        startedFirstWander = true;
        startedWander = false;
    }
}
