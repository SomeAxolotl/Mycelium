using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Sporeburst : Skill
{
    [SerializeField] private float burstRadius = 3f;

    public override void DoSkill()
    {
        DamageEnemies();
        EndSkill();
    }

    void DamageEnemies()
    {
        int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");

        Collider[] colliders = Physics.OverlapSphere(transform.position, burstRadius, enemyLayerMask);

        float damage = finalSkillValue;
        foreach (Collider collider in colliders)
        {
            EnemyHealth enemyHealth = collider.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                NavMeshAgent navMeshAgent = collider.gameObject.GetComponent<NavMeshAgent>();
                if (navMeshAgent != null)
                {
                    navMeshAgent.enabled = false;
                    StartCoroutine(ReactivateNavMeshAfterDelay(navMeshAgent, 2f));
                }

                enemyHealth.EnemyTakeDamage(damage);
                HealPlayer(collider.gameObject);
                Debug.Log("Sporeburst hit!");
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

    IEnumerator ReactivateNavMeshAfterDelay(NavMeshAgent navMeshAgent, float delay)
    {
        yield return new WaitForSeconds(delay);
        if (navMeshAgent != null)
        {
            navMeshAgent.enabled = true;
        }
    }
}

