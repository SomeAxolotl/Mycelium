using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossNavigation : MonoBehaviour
{
    public NavMeshAgent meleeAgent;
    public NavMeshAgent rangedAgent;
    private NavMeshAgent activeAgent;
    private Collider[] playerColliders;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;
    private Transform player;
    [SerializeField] private float fieldOfView = 60f;
    [SerializeField] private float detectionRange = 25f;
    [SerializeField] private float detectionBuffer = 2f;
    private bool canChase = true;
    // Start is called before the first frame update
    void Start()
    {
        activeAgent = meleeAgent;
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

            if (Vector3.Angle(transform.forward, dirToPlayer) < fieldOfView && !Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleLayer) ||
                dstToPlayer <= (activeAgent.stoppingDistance + detectionBuffer) && !Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleLayer))
            {
                if (canChase)
                {
                    StartCoroutine(ChasePlayer());
                }
                transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10f);
                transform.position = activeAgent.transform.position;
            }
        }
    }
    IEnumerator ChasePlayer()
    {
        canChase = false;
        activeAgent.SetDestination(player.position);
        yield return new WaitForSeconds(0.5f);
        canChase = true;
    }
}
