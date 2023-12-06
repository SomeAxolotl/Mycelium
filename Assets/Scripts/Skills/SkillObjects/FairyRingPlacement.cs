using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FairyRingPlacement : FairyRing
{
    [SerializeField] private float damageOverTimeDuration = 7f;
    [SerializeField] private float speedReduction = 0.5f;
    [SerializeField] private float timeElapsed = 0f;
    private IEnumerator DamageOverTime(EnemyHealth enemyHealth, float damage)
    {
        while (timeElapsed < damageOverTimeDuration)
        {
            
            float damageInterval = damage / damageOverTimeDuration;

            Debug.Log("Applying damage: " + damageInterval);
            enemyHealth.EnemyTakeDamage(damageInterval);
            yield return new WaitForSeconds(1f);
            timeElapsed ++;
        }
        Destroy(gameObject);
    }
    void OnTriggerEnter(Collider other)
    {
        float damage = finalSkillValue;
        if (other.CompareTag("Enemy") || other.CompareTag("Boss"))
        {
            EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                NavMeshAgent navMeshAgent = other.gameObject.GetComponent<NavMeshAgent>();
                if (navMeshAgent != null)
                {
                    navMeshAgent.speed = speedReduction * 4f;
                }
                if (enemyHealth != null)
                {
                    StartCoroutine(DamageOverTime(enemyHealth, damage));
                    Debug.Log("Fairy Ring hit!");
                }

            }
        }
    }
}
