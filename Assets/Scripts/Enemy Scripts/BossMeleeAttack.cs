using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossMeleeAttack : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private BossHealth bossHealth;
    private Collider[] playerColliders;
    private GameObject hitbox;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;
    private Transform player;
    private bool canAttack = true;
    [SerializeField] private float attackCooldown = 1.8f;
    private float attackWindup = 1f;
    [SerializeField] private float fieldOfView = 60f;
    [SerializeField] private float detectionRange = 20f;
    IEnumerator attack;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponentInParent<NavMeshAgent>();
        bossHealth = GetComponentInParent<BossHealth>();
        hitbox = this.transform.GetChild(1).gameObject;
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
            if (bossHealth.damaged)
            {
                StopCoroutine(Attack());
                attack = Attack();
                transform.position = transform.position;
                attackWindup = 1.2f;
                canAttack = true;
            }
            else
            {
                // Check FOV before attempting to attack
                if (Vector3.Angle(transform.forward, dirToPlayer) < fieldOfView / 2f &&
                    !Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleLayer))
                {
                    if (bossHealth.damaged)
                    {
                        StopCoroutine(Attack());
                        StartCoroutine(Attack());
                        transform.position = transform.position;
                        attackWindup = 1.2f;
                        canAttack = true;
                    }
                    else
                    {
                        if (dstToPlayer <= navMeshAgent.stoppingDistance + detectionRange)
                        {
                            attackWindup -= Time.deltaTime;
                            if (attackWindup <= 0)
                            {
                                StartCoroutine(Attack());
                            }
                        }
                        else
                        {
                            attackWindup = 1f;
                        }
                    }
                }
            }
            IEnumerator Attack()
            {
                if (canAttack)
                {
                    canAttack = false;

                    hitbox.GetComponent<Collider>().enabled = true;
                    hitbox.GetComponent<Renderer>().enabled = true;

                    yield return new WaitForSeconds(0.2f); // Attack animation will go on this line!

                    hitbox.GetComponent<Collider>().enabled = false;
                    hitbox.GetComponent<Renderer>().enabled = false;

                    yield return new WaitForSeconds(attackCooldown);
                    canAttack = true;
                }
            }
        }
    }
}

