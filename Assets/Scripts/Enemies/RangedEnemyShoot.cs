using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemyShoot : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private EnemyKnockback enemyKnockback;
    private EnemyHealth enemyHealth;
    private Collider[] playerColliders;
    public LayerMask playerLayer;
    public LayerMask obstacleLayer;
    private Transform player;
    public Transform centerPoint;
    public Transform launchPoint;
    private bool canAttack = true;
    private bool isAttacking = false;
    private bool stunned = false;
    private float attackDistance = 15f;
    [SerializeField] private float attackCooldown = 3f;
    public GameObject projectile;
    Coroutine attackCoroutine;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        enemyKnockback = GetComponent<EnemyKnockback>();
        enemyHealth = GetComponent<EnemyHealth>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        playerColliders = Physics.OverlapSphere(transform.position, 25f, playerLayer);
        foreach (var playerCollider in playerColliders)
        {
            player = playerCollider.transform.Find("CenterPoint");
            Vector3 dirToPlayer = (player.position - launchPoint.position).normalized;
            float dstToPlayer = Vector3.Distance(launchPoint.position, player.position);
            if (enemyKnockback.damaged && !stunned)
            {
                StartCoroutine(CancelAttack());
                Debug.Log("stunned");
            }
            else
            {
                if (Vector3.Angle(transform.forward, dirToPlayer) < 20f && !Physics.Raycast(launchPoint.position, dirToPlayer, dstToPlayer, obstacleLayer) && dstToPlayer <= attackDistance && canAttack)
                {
                    StartCoroutine(Attack());
                    Debug.Log("attackkk");
                }
            }
        }
    }
    
    IEnumerator Attack()
    {
        if (!isAttacking && canAttack)
        {
            canAttack = false;
            isAttacking = true;
            attackCoroutine = StartCoroutine(AttackLogic());
        }

        yield return null;
    }
    IEnumerator AttackLogic()
    {
        isAttacking = true;
        if (!isAttacking)
        {
            yield break;
        }
        animator.SetTrigger("Attack");
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime > .6f);
        if(enemyHealth.currentHealth > 0 && isAttacking)
        {
            Vector3 dirToPlayer = player.position - launchPoint.position;
            GameObject tempProj = Instantiate(projectile, launchPoint.position, transform.rotation);
            tempProj.transform.right = new Vector3(dirToPlayer.x, dirToPlayer.y + 1f, dirToPlayer.z);
            tempProj.GetComponent<Rigidbody>().velocity = dirToPlayer.normalized * 18f;
        }
        isAttacking = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;

    }
    IEnumerator CancelAttack()
    {
        if(attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            isAttacking = false;
            stunned = true;
            transform.position = transform.position;
            navMeshAgent.speed = 3f;
            yield return new WaitUntil(() => enemyKnockback.damaged == false);
            yield return new WaitForSeconds(.25f);
            canAttack = true;
            stunned = false;
        }
    }
}
