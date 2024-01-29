using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyAttack : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private EnemyNavigation enemyNavigation;
    private EnemyKnockback enemyKnockback;
    private MeleeEnemyHitbox meleeEnemyHitbox;
    private Collider[] playerColliders;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;
    private Transform player;
    private bool canAttack = true;
    private bool isAttacking = false;
    private bool attackStarted = false;
    private float attackCooldown = 1.5f;
    private float attackStartup;
    private float resetAttack;
    [SerializeField] private float damage = 20f;
    [SerializeField] private float knockbackForce = 50f;
    IEnumerator attack;
    Rigidbody rb;
    Animator animator;
    List<GameObject> playerHit = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyKnockback = GetComponent<EnemyKnockback>();
        enemyNavigation = GetComponent<EnemyNavigation>();
        meleeEnemyHitbox = GetComponentInChildren<MeleeEnemyHitbox>();
        attack = this.Attack();
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
            if(enemyKnockback.damaged || (animator.GetCurrentAnimatorStateInfo(0).IsName("Attack") && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f) || resetAttack >= 2f)
            {
                CancelAttack();
            }
            else
            {
                if (Vector3.Angle(transform.forward, dirToPlayer) < 20f && !Physics.Raycast(transform.position, dirToPlayer, dstToPlayer, obstacleLayer))
                {
                    attackStartup += Time.deltaTime;
                    if (canAttack && attackStartup >= 0.5f)
                    {     
                        StartCoroutine(Attack());
                    }
                }
                else
                {
                    attackStartup = 0f;
                }
            }
        }
        if (attackStarted)
        {
            resetAttack += Time.deltaTime;
        }
        else
        {
            resetAttack = 0;
        }
    }
    IEnumerator Attack()
    {
        canAttack = false;
        attackStarted = true;
        enemyNavigation.attacking = true;
        navMeshAgent.speed = 0f;
        navMeshAgent.angularSpeed = 0f;
        navMeshAgent.stoppingDistance = 0f;
        enemyNavigation.animator.speed = 0f;
        Vector3 chargeTarget = player.position;
        Vector3 dirToPlayer = (chargeTarget - transform.position);
        var newRotation = Quaternion.LookRotation(dirToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10f);
        yield return new WaitForSeconds(.2f);
        navMeshAgent.SetDestination(chargeTarget);
        enemyNavigation.animator.speed = 5f;
        navMeshAgent.speed = 25f;
        navMeshAgent.acceleration = 25f;
        isAttacking = true;
        yield return new WaitUntil(() => Vector3.Distance(transform.position, chargeTarget) < 2f);
        animator.SetTrigger("Attack");
        isAttacking = false;
        yield return new WaitUntil(() => Vector3.Distance(transform.position, chargeTarget) < 1f);
        navMeshAgent.speed = 6f;
        navMeshAgent.angularSpeed = 500f;
        enemyNavigation.animator.speed = 1f;
        navMeshAgent.acceleration = 7f;
        enemyNavigation.attacking = false;
        navMeshAgent.stoppingDistance = 1f;
        playerHit.Clear();
        attackStarted = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
    void CancelAttack()
    {
        StopAllCoroutines();
        attack = Attack();
        transform.position = transform.position;
        playerHit.Clear();
        navMeshAgent.speed = 6f;
        navMeshAgent.angularSpeed = 500f;
        navMeshAgent.acceleration = 7f;
        enemyNavigation.animator.speed = 1f;
        attackStartup = -0.25f;
        canAttack = true;
        isAttacking = false;
        enemyNavigation.attacking = false;
        attackStarted = false;
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "currentPlayer" && other.gameObject.GetComponentInParent<PlayerController>().isInvincible == false && !playerHit.Contains(other.gameObject) && isAttacking)
        {
            other.gameObject.GetComponentInParent<PlayerHealth>().PlayerTakeDamage(damage);
            other.gameObject.GetComponentInParent<PlayerController>().Knockback(this.gameObject, knockbackForce);
            playerHit.Add(other.gameObject);
        }
    }
}
