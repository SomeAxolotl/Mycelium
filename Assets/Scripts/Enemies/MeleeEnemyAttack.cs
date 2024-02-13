using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeleeEnemyAttack : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private EnemyNavigation enemyNavigation;
    private EnemyKnockback enemyKnockback;
    public LayerMask playerLayer;
    private bool canAttack = true;
    private bool isAttacking = false;
    private bool attackStarted = false;
    private bool playerDamaged = false;
    [SerializeField] private float attackCooldown = 2f;
    private float resetAttack;
    [SerializeField] private float damage = 20f;
    [SerializeField] private float knockbackForce = 50f;
    IEnumerator attack;
    Animator animator;
    List<GameObject> playerHit = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyKnockback = GetComponent<EnemyKnockback>();
        enemyNavigation = GetComponent<EnemyNavigation>();
        attack = this.Attack();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(enemyKnockback.damaged)
        {
            CancelAttack();
        }
        else
        {
            if (enemyNavigation.playerSeen && canAttack && navMeshAgent != null)
            {  
                StartCoroutine(Attack());
            }
        }

        if(attackStarted && navMeshAgent.speed == 0f)
        {
            Vector3 player = GameObject.FindWithTag("currentPlayer").transform.position;
            Vector3 dirToPlayer = (player - transform.position);
            var newRotation = Quaternion.LookRotation(dirToPlayer);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 10f);
        }

        if(isAttacking)
        {
            resetAttack += Time.deltaTime;
        }
        else
        {
            resetAttack = 0f;
        }
    }
    IEnumerator Attack()
    {
        canAttack = false;
        attackStarted = true;
        enemyNavigation.attacking = true;
        navMeshAgent.speed = 0f;
        navMeshAgent.stoppingDistance = 0f;
        enemyNavigation.animator.speed = 0f;
        Vector3 chargeTarget = GameObject.FindWithTag("currentPlayer").transform.position;
        yield return new WaitForSeconds(.75f);
        SoundEffectManager.Instance.PlaySound("Beetle Charge", transform.position);
        navMeshAgent.SetDestination(chargeTarget);
        enemyNavigation.animator.speed = 3f;
        navMeshAgent.speed = 20f;
        navMeshAgent.acceleration = 20f;
        isAttacking = true;
        yield return new WaitUntil(() => Vector3.Distance(transform.position, chargeTarget) <= .25f || playerDamaged || resetAttack > 1f);
        enemyNavigation.animator.speed = 1f;
        animator.SetTrigger("Attack");
        isAttacking = false;
        playerDamaged = false;
        navMeshAgent.speed = 6f;
        navMeshAgent.acceleration = 7f;
        enemyNavigation.attacking = false;
        navMeshAgent.stoppingDistance = 1f;
        playerHit.Clear();
        attackStarted = false;
        enemyNavigation.playerSeen = false;
        enemyNavigation.startedPatrol = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
    public void CancelAttack()
    {
        StopAllCoroutines();
        attack = Attack();
        transform.position = transform.position;
        isAttacking = false;
        playerDamaged = false;
        navMeshAgent.speed = 6f;
        navMeshAgent.acceleration = 7f;
        enemyNavigation.attacking = false;
        navMeshAgent.stoppingDistance = 1f;
        playerHit.Clear();
        attackStarted = false;
        enemyNavigation.playerSeen = false;
        enemyNavigation.startedPatrol = false;
        canAttack = true;
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "currentPlayer" && other.gameObject.GetComponentInParent<PlayerController>().isInvincible == false && !playerHit.Contains(other.gameObject) && isAttacking)
        {
            playerDamaged = true;
            other.gameObject.GetComponentInParent<PlayerHealth>().PlayerTakeDamage(damage);
            other.gameObject.GetComponentInParent<PlayerController>().Knockback(this.gameObject, knockbackForce);
            playerHit.Add(other.gameObject);
        }
    }
}
