using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemyShoot : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private EnemyKnockback enemyKnockback;
    private Collider[] playerColliders;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;
    private Transform player;
    private bool canAttack = true;
    private bool isAttacking = false;
    private bool windupStarted = false;
    [SerializeField] private float attackCooldown = 1.5f;
    private float attackWindup = .6f;
    public GameObject projectile;
    IEnumerator attack;
    
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyKnockback = GetComponent<EnemyKnockback>();
        attack = this.Attack();
    }

    // Update is called once per frame
    void Update()
    {
        playerColliders = Physics.OverlapSphere(transform.position, 25f, playerLayer);
        foreach (var playerCollider in playerColliders)
        {
            player = playerCollider.transform;
            Vector3 dirToPlayer = (player.position - transform.position).normalized;
            float dstToPlayer = Vector3.Distance(transform.position, player.position);
            if (enemyKnockback.damaged)
            {
                CancelAttack();
            }
            else
            {
                if (Vector3.Angle(transform.forward, dirToPlayer) < 20f && !Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleLayer))
                {
                    if ((dstToPlayer - .5f) <= navMeshAgent.stoppingDistance && canAttack && !windupStarted)
                    {
                        StartCoroutine(AttackWindup());
                    }
                }
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
        isAttacking = true;
        navMeshAgent.speed = 3f;
        attackWindup = .6f;
        Vector3 dirToPlayer = new Vector3(player.transform.position.x, player.transform.position.y + 0.5f, player.transform.position.z) - transform.position;
        if (!isAttacking)
        {
            yield break;
        }
        GameObject tempProj = Instantiate(projectile, transform.position, transform.rotation);
        tempProj.transform.right = dirToPlayer;
        tempProj.GetComponent<Rigidbody>().velocity = dirToPlayer.normalized * 18f;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
        isAttacking = false;
        windupStarted = false;
    }
    void CancelAttack()
    {
        StopAllCoroutines();
        attack = Attack();
        transform.position = transform.position;
        attackWindup = .2f;
        navMeshAgent.speed = 3f;
        canAttack = true;
        isAttacking = false;
        windupStarted = false;
    }
}
