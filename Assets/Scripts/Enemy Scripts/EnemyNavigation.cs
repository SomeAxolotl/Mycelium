using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavigation : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private bool playerSeen;
    private bool patrolStart = false;
    private float patrolCooldown = 1f;
    private bool isRotatingLeft = false;
    private bool isRotatingRight = false;
    private bool isWalking = false;
    private Collider[] playerColliders;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;
    private Transform player;
    [SerializeField] private float fieldOfView = 60f;
    [SerializeField] private float detectionRange = 25f;
    [SerializeField] private float detectionBuffer = 2f;
    IEnumerator patrol;
    IEnumerator restartPatrol;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        patrol = this.Patrol();
        restartPatrol = this.RestartPatrol();
    }

    // Update is called once per frame
    void Update()
    {
        playerColliders = Physics.OverlapSphere(transform.position, detectionRange, playerLayer);
        foreach (var playerCollider in playerColliders)
        {
            player = playerCollider.transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float dstToPlayer = Vector3.Distance(transform.position, player.position);
            var newRotation = Quaternion.LookRotation(dirToPlayer);
            
            if (Vector3.Angle(transform.forward, dirToPlayer) < fieldOfView && !Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleLayer) || dstToPlayer <= (navMeshAgent.stoppingDistance + detectionBuffer) && !Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleLayer))
            {
                //Debug.Log("Spotted!");
                playerSeen = true;
                StopCoroutine(patrol);
                patrol = Patrol();
                StopCoroutine(restartPatrol);
                restartPatrol = RestartPatrol();
                patrolStart = false;
                patrolCooldown = 1f;
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(player.position);
                transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10f);
            }
            else
            {
                playerSeen = false;
                navMeshAgent.isStopped = true;
                navMeshAgent.ResetPath();
            }
        }
        
        if(playerSeen == false && patrolStart == false)
        {
            patrolCooldown -= Time.deltaTime;
            if(patrolCooldown <= 0)
            {
                StartCoroutine("Patrol");
            }
        }

        if (isRotatingRight == true)
        {
            transform.Rotate(transform.up * Time.deltaTime * 100f);
        }

        if (isRotatingLeft == true)
        {
            transform.Rotate(transform.up * Time.deltaTime * -100f);
        }

        if (isWalking == true)
        {
            if(!Physics.Raycast(transform.position, transform.forward, 2f, obstacleLayer))
            {
                transform.position += transform.forward * 5f * Time.deltaTime;
            }
            else
            {
                StartCoroutine("RestartPatrol");
            }
        }
    }
    IEnumerator Patrol()
    {
        patrolStart = true;
        int rotTime = Random.Range(1, 3);
        int rotateWait = Random.Range(1, 3);
        int rotateLorR = Random.Range(1, 2);
        int walkWait = Random.Range(1, 3);
        int walkTime = Random.Range(1, 4);

        yield return new WaitForSeconds(walkWait);
        isWalking = true;
        yield return new WaitForSeconds(walkTime);
        isWalking = false;
        yield return new WaitForSeconds(rotateWait);
        if (rotateLorR == 1)
        {
            isRotatingRight = true;
            yield return new WaitForSeconds(rotTime);
            isRotatingRight = false;
        }
        if (rotateLorR == 2)
        {
            isRotatingLeft = true;
            yield return new WaitForSeconds(rotTime);
            isRotatingLeft = false;
        }
        patrolStart = false;
        patrolCooldown = 1f;
    }
    IEnumerator RestartPatrol()
    {
        StopCoroutine(patrol);
        patrol = Patrol();
        transform.rotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y + 180f * Time.deltaTime, transform.rotation.z);
        yield return new WaitForSeconds(1f);
        patrolStart = false;
        patrolCooldown = 1f;
    }
}
