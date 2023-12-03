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
    private Transform hitbox;
    private GameObject thisHitbox;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;
    private Transform player;
    private bool canAttack = true;
    private bool isAttacking = false;
    private bool windupStarted = false;
    [SerializeField] private float attackCooldown = 2f;
    private float attackWindup = .75f;
    [SerializeField] float lungeDistance = 0.4f;
    [SerializeField] float lungeDuration = 0.15f;
    IEnumerator attack;
    IEnumerator windup;
    Rigidbody rb;
    Animator animator;
    
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyKnockback = GetComponent<EnemyKnockback>();
        enemyNavigation = GetComponent<EnemyNavigation>();
        hitbox = transform.Find("MeleeHitbox");
        thisHitbox = hitbox.gameObject;
        attack = this.Attack();
        windup = this.AttackWindup();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
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
        attackWindup = .75f;
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

        thisHitbox.GetComponent<Collider>().enabled = true;

        animator.SetBool("Attack", true);

        yield return new WaitForSeconds(0.2f); //Attack animation will go here!

        thisHitbox.GetComponent<Collider>().enabled = false;

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
        
        animator.SetBool("Attack", true);

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
        attackWindup = .75f;
        navMeshAgent.speed = 5f;
        canAttack = true;
        isAttacking = false;
        windupStarted = false;
    }
}
