using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemyShoot : EnemyAttack
{
    private EnemyKnockback enemyKnockback;
    private ReworkedEnemyNavigation reworkedEnemyNavigation;
    private Transform player;
    public Transform launchPoint;
    private bool canAttack = true;
    private float attackCooldown = 3f;
    private float attackWindupTime = 1f;
    public GameObject projectile;
    IEnumerator attack;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        enemyKnockback = GetComponent<EnemyKnockback>();
        reworkedEnemyNavigation = GetComponent<ReworkedEnemyNavigation>();
        attack = this.Attack();
        animator = GetComponent<Animator>();
        player = GameObject.FindWithTag("currentPlayer").transform.Find("CenterPoint");
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyKnockback.damaged)
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


    }
    private void FixedUpdate()
    {
        if (reworkedEnemyNavigation.playerSeen)
        {
            Vector3 dirToPlayer = player.position - transform.position;
            Quaternion desiredRotation = Quaternion.LookRotation(dirToPlayer);
            float desiredYRotation = desiredRotation.eulerAngles.y;
            Quaternion targetRotation = Quaternion.Euler(0f, desiredYRotation, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8f);
        }
    }

    public override IEnumerator Attack()
    {
        canAttack = false;
        yield return new WaitForSeconds(attackWindupTime);
        animator.SetTrigger("Attack");
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime > .6f);
        Vector3 dirToPlayer = (player.position - launchPoint.position).normalized;
        GameObject tempProj = Instantiate(projectile, launchPoint.position, Quaternion.identity);
        tempProj.transform.right = dirToPlayer;
        tempProj.GetComponent<Rigidbody>().velocity = dirToPlayer * 15f;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
    public override void CancelAttack()
    {
        StopAllCoroutines();
        attack = Attack();
        canAttack = true;
    }
}
