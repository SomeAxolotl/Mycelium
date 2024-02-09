using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Sporeburst : Skill
{
    [SerializeField] private float burstRadius = 3f;
    [SerializeField] private float stunDuration = 2f;
    public override void DoSkill()
    {
        DoSporeburst();
        EndSkill();
    }

    void DoSporeburst()
    {
        int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");

        Collider[] enemies = Physics.OverlapSphere(transform.position, burstRadius, enemyLayerMask);
        float damage = finalSkillValue;
        foreach (var enemy in enemies)
        {
            if (enemy.gameObject.GetComponent<EnemyHealth>() != null)
            {
                enemy.gameObject.GetComponent<EnemyHealth>().EnemyTakeDamage(damage);
                HealPlayer(enemy.gameObject);
                Debug.Log("Sporeburst hit!");
            }

            if (enemy.gameObject.GetComponent<MeleeEnemyAttack>() != null)
            {
                enemy.gameObject.GetComponent<MeleeEnemyAttack>().CancelAttack();
                StartCoroutine(ReactivateMelee(enemy.gameObject.GetComponent<MeleeEnemyAttack>()));
                enemy.gameObject.GetComponent<MeleeEnemyAttack>().enabled = false;
            }

            if (enemy.gameObject.GetComponent<RangedEnemyShoot>() != null)
            {
                enemy.gameObject.GetComponent<RangedEnemyShoot>().StartCoroutine("CancelAttack");
                StartCoroutine(ReactivateRanged(enemy.gameObject.GetComponent<RangedEnemyShoot>()));
                enemy.gameObject.GetComponent<RangedEnemyShoot>().enabled = false;
            }

            if (enemy.gameObject.GetComponent<EnemyNavigation>() != null)
            {
                StartCoroutine(ReactivateNavigation(enemy.gameObject.GetComponent<EnemyNavigation>(),enemy.gameObject.GetComponent<NavMeshAgent>()));
                enemy.gameObject.GetComponent<EnemyNavigation>().enabled = false;
                enemy.gameObject.GetComponent<NavMeshAgent>().enabled = false;
                enemy.gameObject.GetComponent<Animator>().SetBool("IsMoving", false);
            }
        }
    }
    void HealPlayer(GameObject enemy)
    {
        float damage = finalSkillValue;
        float healingAmount = 0.5f * damage;
        if (enemy != null)
        {
            GameObject.FindWithTag("PlayerParent").GetComponent<PlayerHealth>().PlayerHeal(healingAmount);
        }
    }
    IEnumerator ReactivateMelee(MeleeEnemyAttack meleeEnemyAttack)
    {
        yield return new WaitForSeconds(stunDuration);
        meleeEnemyAttack.enabled = true;
    }
    IEnumerator ReactivateRanged(RangedEnemyShoot rangedEnemyShoot)
    {
        yield return new WaitForSeconds(stunDuration);
        rangedEnemyShoot.enabled = true;
    }
    IEnumerator ReactivateNavigation(EnemyNavigation enemyNavigation, NavMeshAgent navMeshAgent)
    {
        yield return new WaitForSeconds(stunDuration);
        enemyNavigation.gameObject.GetComponent<Animator>().SetBool("IsMoving", true);
        navMeshAgent.enabled = true;
        enemyNavigation.enabled = true;
    }
}

