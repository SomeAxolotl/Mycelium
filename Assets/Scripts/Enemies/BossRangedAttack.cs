using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossRangedAttack : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Collider[] playerColliders;
    private Transform player;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;
    public GameObject projectile;
    private bool canAttack = true;
    private bool windupStarted = false;
    private float attackWindup = .5f;
    [SerializeField] private float attackCooldown = 2f;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        playerColliders = Physics.OverlapSphere(transform.position, 20f, playerLayer);
        foreach (var playerCollider in playerColliders)
        {
            player = playerCollider.transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float dstToPlayer = Vector3.Distance(transform.position, player.position);
            if (Vector3.Angle(transform.forward, dirToPlayer) < 30f && !Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleLayer) && (dstToPlayer - 0.2f) <= navMeshAgent.stoppingDistance && canAttack && !windupStarted)
            {
                StartCoroutine(AttackWindup());
            }
        }
    }
    IEnumerator AttackWindup()
    {
        windupStarted = true;
        navMeshAgent.speed = 0f;
        yield return new WaitForSeconds(attackWindup);
        StartCoroutine(Attack());
    }
    IEnumerator Attack()
    {
        canAttack = false;
        windupStarted = false;
        navMeshAgent.speed = 3f;
        attackWindup = .8f;

        Vector3 dirToPlayer = new Vector3(player.transform.position.x, player.transform.position.y + 0.5f, player.transform.position.z) - transform.position;
        GameObject tempProj = Instantiate(projectile, transform.position, transform.rotation);
        tempProj.transform.right = dirToPlayer;
        tempProj.GetComponent<Rigidbody>().velocity = dirToPlayer.normalized * 18f;

        yield return new WaitForSeconds(attackCooldown);
        windupStarted = false;
        canAttack = true;
    }
}
