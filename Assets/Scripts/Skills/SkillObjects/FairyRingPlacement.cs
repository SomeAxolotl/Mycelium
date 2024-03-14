using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FairyRingPlacement : FairyRing
{
    [SerializeField] private float damageOverTimeDuration = 7f;
    [SerializeField] public float damage = 0f;
    [SerializeField] private float speedReduction = 0.5f;

    // Flag to track if an enemy is within the fairy ring collider
    private bool enemyInsideFairyRing = false;

    void Start()
    {
        StartCoroutine(DestroyAfterTime());
    }

    private IEnumerator DamageOverTime(EnemyHealth enemyHealth, float damage)
    {
        float timeElapsed = 0f;
        float damageInterval = damage / damageOverTimeDuration;
        while (timeElapsed < damageOverTimeDuration && enemyInsideFairyRing)
        {
            Debug.Log("Applying damage: " + damageInterval);
            enemyHealth.EnemyTakeDamage(damageInterval);
            yield return new WaitForSeconds(1f);
            timeElapsed++;
        }
    }

    private IEnumerator DestroyAfterTime()
    {
        yield return new WaitForSeconds(damageOverTimeDuration);
        Destroy(this.gameObject);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Boss"))
        {
            enemyInsideFairyRing = true; // Set flag when an enemy enters the fairy ring collider

            EnemyHealth enemyHealth = other.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                ReworkedEnemyNavigation enemyNav = other.gameObject.GetComponent<ReworkedEnemyNavigation>();
                if (enemyNav != null && enemyInsideFairyRing)
                {
                    enemyNav.moveSpeed = speedReduction * 4f;
                }
                StartCoroutine(DamageOverTime(enemyHealth, damage));
                Debug.Log("Fairy Ring hit!");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Boss"))
        {
            enemyInsideFairyRing = false; // Reset flag when an enemy exits the fairy ring collider
        }
    }
}
