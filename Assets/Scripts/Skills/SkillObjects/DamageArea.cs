using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DamageArea : Mycotoxins
{
    [SerializeField] private float damageOverTimeDuration = 3f;
    [SerializeField] private float timeElapsed = 0f;
    private IEnumerator DamageOverTime(EnemyHealth enemyHealth, float damage)
    {
        while (timeElapsed < damageOverTimeDuration)
        {

            float damageInterval = damage / damageOverTimeDuration;

            Debug.Log("Applying damage: " + damageInterval);
            enemyHealth.EnemyTakeDamage(damageInterval);
            yield return new WaitForSeconds(1f);
            timeElapsed++;
        }
    }
    void OnTriggerEnter(Collider other)
    {
        float damage = finalSkillValue;
        if (other.CompareTag("Enemy") || other.CompareTag("Boss"))
        {
            EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                StartCoroutine(DamageOverTime(enemyHealth, damage));
                Debug.Log("Toxins hit!");
            }
        }
    }
}

