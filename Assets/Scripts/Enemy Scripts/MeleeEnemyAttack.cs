using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyAttack : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private EnemyNavigation enemyNavigation;
    private EnemyKnockback enemyKnockback;
    private Collider[] playerColliders;
    private GameObject hitbox;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;
    private Transform player;
    private bool canAttack = true;
    private bool isAttacking = false;
    private bool windupStarted = false;
    [SerializeField] private float attackCooldown = 2f;
    private float attackWindup = .5f;
    [SerializeField] float lungeDistance = 0.4f;
    [SerializeField] float lungeDuration = 0.15f;
    IEnumerator attack;
    IEnumerator windup;
    Rigidbody rb;
    
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyKnockback = GetComponent<EnemyKnockback>();
        enemyNavigation = GetComponent<EnemyNavigation>();
        hitbox = this.transform.GetChild(1).gameObject;
        attack = this.Attack();
        windup = this.AttackWindup();
        rb = GetComponent<Rigidbody>();
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
            if(enemyKnockback.damaged)
            {
                CancelAttack();
            }
            else
            {
                if (Vector3.Angle(transform.forward, dirToPlayer) < 20f && !Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleLayer))
                {
                    if((dstToPlayer - 1f) <= navMeshAgent.stoppingDistance && canAttack && !windupStarted)
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
        navMeshAgent.speed = 5f;
        attackWindup = .5f;
        Vector3 startPosition = transform.position;

        //Lunge Forwards
        for (float t = 0; t < lungeDuration; t += Time.deltaTime)
        {
            if(!isAttacking)
            {
                yield break;
            }
            float progress = t / lungeDuration;
            transform.position = Vector3.Lerp(startPosition, startPosition + transform.forward * lungeDistance, progress);
            yield return null;
        }

        hitbox.GetComponent<Collider>().enabled = true;
        hitbox.GetComponent<Renderer>().enabled = true;

        yield return new WaitForSeconds(0.2f); //Attack animation will go here!

        hitbox.GetComponent<Collider>().enabled = false;
        hitbox.GetComponent<Renderer>().enabled = false;

        //Lunge Backwards
        for (float t = 0; t < lungeDuration; t += Time.deltaTime)
        {
            if (!isAttacking)
            {
                yield break;
            }
            float progress = t / lungeDuration;
            transform.position = Vector3.Lerp(startPosition + transform.forward * lungeDistance, startPosition, progress);
            yield return null;
        }
        transform.position = startPosition;
        yield return new WaitForSeconds(attackCooldown);
        windupStarted = false;
        canAttack = true;
        isAttacking = false;
    }
    void CancelAttack()
    {
        StopAllCoroutines();
        attack = Attack();
        windup = AttackWindup();
        transform.position = transform.position;
        attackWindup = .3f;
        navMeshAgent.speed = 5f;
        canAttack = true;
        isAttacking = false;
        windupStarted = false;
    }
}
