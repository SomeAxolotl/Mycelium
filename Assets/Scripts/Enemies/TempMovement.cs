using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TempMovement : MonoBehaviour
{
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
    [SerializeField] private float detectionRange = 30f;
    [SerializeField] private float backwardsDetectionRange = 15f;
    public bool undergrowthSpeed;

    // turning
    /*[HideInInspector] */public bool playerIsRight;
    /*[HideInInspector] */public bool playerIsLeft;
    public float previousRotationY;

    // Start is called before the first frame update
    void Start()
    {
        //transform.rotation = Quaternion.Euler(46, 0, 0);
        previousPosition = transform.position;
        previousRotationY = transform.eulerAngles.y;
        playerIsRight = false;
        playerIsLeft = false;
    }

    // Update is called once per frame
    void Update()
    {
        // transform.rotation = Quaternion.Euler(46, 0, 0);
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

            float angleToPlayer = Vector3.SignedAngle(transform.forward, dirToPlayer, Vector3.up);

            if ((angleToPlayer < fieldOfView && !Physics.Raycast(new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), dirToPlayer, dstToPlayer, obstacleLayer)) ||
                dstToPlayer <= backwardsDetectionRange)
            {
                playerSeen = true;
                startedPatrol = false;

                if (!attacking)
                {
                    // newRotation = Quaternion.LookRotation(dirToPlayer);
                    // newRotation.eulerAngles = new Vector3(46f, newRotation.eulerAngles.y, newRotation.eulerAngles.z);
                    // transform.rotation = newRotation;

                    // float deltaYRotation = Mathf.DeltaAngle(previousRotationY, transform.eulerAngles.y);
                    // turningRight = deltaYRotation > 0;
                    // turningLeft = deltaYRotation < 0;
                    // previousRotationY = transform.eulerAngles.y;

                    if (angleToPlayer > 0)
                    {
                        // Perform the action when the player is to the right
                        playerIsRight = true;
                        playerIsLeft = false;
                    }

                    if (angleToPlayer < 0)
                    {
                        // Perform the action when the player is to the right
                        playerIsLeft = true;
                        playerIsRight = false;
                    }
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
    }
}
