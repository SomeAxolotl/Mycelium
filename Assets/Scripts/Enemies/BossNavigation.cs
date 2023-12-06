using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossNavigation : MonoBehaviour
{
    public NavMeshAgent meleeAgent;
    public NavMeshAgent rangedAgent;
    private NavMeshAgent activeAgent;
    public GameObject meleeSide;
    public GameObject rangedSide;
    private GameObject activeSide;
    private GameObject inactiveSide;
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
        activeSide = meleeSide;
        inactiveSide = rangedSide;
        InvokeRepeating("ChangeSides", 15f, 15f);
    }

    // Update is called once per frame
    void Update()
    {
        playerColliders = Physics.OverlapSphere(activeSide.transform.position, detectionRange, playerLayer);
        foreach (var playerCollider in playerColliders)
        {
            player = playerCollider.transform;
            Vector3 dirToPlayer = (player.position - activeSide.transform.position).normalized;
            float dstToPlayer = Vector3.Distance(activeSide.transform.position, player.position);
            var newRotation = Quaternion.LookRotation(dirToPlayer);

            if (Vector3.Angle(transform.forward, dirToPlayer) < fieldOfView && !Physics.Raycast(activeSide.transform.position, dirToPlayer, dstToPlayer, obstacleLayer) ||
                dstToPlayer <= (activeAgent.stoppingDistance + detectionBuffer) && !Physics.Raycast(activeSide.transform.position, dirToPlayer, dstToPlayer, obstacleLayer))
            {
                if (canChase)
                {
                    StartCoroutine(ChasePlayer());
                }

                activeSide.transform.rotation = Quaternion.Slerp(activeSide.transform.rotation, newRotation, Time.deltaTime * 10f);

                //Keeps childrens position back to back
                Vector3 relativePosition = new Vector3(0f, 0f, -1f);
                inactiveSide.transform.localPosition = activeSide.transform.TransformPoint(relativePosition) - transform.position;

                //Keeps childrens rotation back to back
                Vector3 directiontoBack = (activeSide.transform.position - inactiveSide.transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(-directiontoBack, Vector3.up);
                inactiveSide.transform.rotation = lookRotation;

                if (activeAgent == meleeAgent)
                {
                    activeAgent.speed = 5f;
                }
                else if(activeAgent == rangedAgent)
                {
                    activeAgent.speed = 2f;
                }
            }
        }
    }
    IEnumerator ChasePlayer()
    {
        canChase = false;
        activeAgent.SetDestination(player.position);
        yield return new WaitForSeconds(0.3f);
        canChase = true;
    }
    private void ChangeSides()
    {
        if(activeSide == meleeSide)
        {
            activeSide = rangedSide;
            inactiveSide = meleeSide;
            activeAgent = rangedAgent;
        }
        else if(activeSide == rangedSide)
        {
            activeSide = meleeSide;
            inactiveSide = rangedSide;
            activeAgent = meleeAgent;
        }
    }
}
