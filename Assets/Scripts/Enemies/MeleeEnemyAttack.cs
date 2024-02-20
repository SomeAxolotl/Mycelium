using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class MeleeEnemyAttack : MonoBehaviour
{
    private ReworkedEnemyNavigation reworkedEnemyNavigation;
    private EnemyKnockback enemyKnockback;
    private bool canAttack = true;
    private bool isAttacking = false;
    private bool attackStarted = false;
    private bool playerDamaged = false;
    [SerializeField] private float attackCooldown = 2f;
    private float attackWindupTime = .85f;
    private float resetAttack;
    [SerializeField] private float damage = 20f;
    private float knockbackForce = 30f;
    IEnumerator attack;
    Animator animator;
    List<GameObject> playerHit = new List<GameObject>();
    private Transform player;
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        enemyKnockback = GetComponent<EnemyKnockback>();
        reworkedEnemyNavigation = GetComponent<ReworkedEnemyNavigation>();
        attack = this.Attack();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("currentPlayer").transform;
        rb = GetComponent<Rigidbody>();
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
            if (reworkedEnemyNavigation.playerSeen && canAttack)
            {  
                StartCoroutine(Attack());
            }
        }

        if(attackStarted)
        {
            Vector3 dirToPlayer = (player.position - transform.position);
            Quaternion desiredRotation = Quaternion.LookRotation(dirToPlayer);
            float desiredYRotation = desiredRotation.eulerAngles.y;
            Quaternion targetRotation = Quaternion.Euler(0f, desiredYRotation, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8f);
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
        animator.speed = 0f;
        reworkedEnemyNavigation.moveSpeed = 0f;
        yield return new WaitForSeconds(attackWindupTime);
        SoundEffectManager.Instance.PlaySound("Beetle Charge", transform.position);
        animator.speed = 3f;
        attackStarted = false;
        isAttacking = true;
        Transform target = player;
        Vector3 moveDirection = (target.position - transform.position).normalized;
        reworkedEnemyNavigation.moveSpeed = 8f;
        while (Vector3.Distance(transform.position, target.position) > 0.25f && !playerDamaged && resetAttack < 1.5f)
        {
            rb.velocity = new Vector3((moveDirection * reworkedEnemyNavigation.moveSpeed).x, rb.velocity.y, (moveDirection * reworkedEnemyNavigation.moveSpeed).z);
            yield return null;
        }
        animator.speed = 1f;
        animator.SetTrigger("Attack");
        isAttacking = false;
        reworkedEnemyNavigation.moveSpeed = 3f;
        playerDamaged = false;
        playerHit.Clear();
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
    public void CancelAttack()
    {
        StopAllCoroutines();
        attack = Attack();
        animator.speed = 1f;
        isAttacking = false;
        reworkedEnemyNavigation.moveSpeed = 3f;
        playerDamaged = false;
        playerHit.Clear();
        attackStarted = false;
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
