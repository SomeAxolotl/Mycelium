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
                float healingAmount = 0.5f * damage;
                enemyHealth.EnemyTakeDamage(damage);
                HealPlayer(healingAmount);
                Debug.Log("Sporeburst hit!");
            }
        }
    }
    void HealPlayer(float healingAmount)
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.PlayerHeal(healingAmount);
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

