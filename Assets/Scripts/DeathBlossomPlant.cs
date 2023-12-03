using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBlossomPlant : DeathBlossom
{
    [SerializeField] private float destroyTime = 2f;
    [SerializeField] private float burstRadius = 10f;
    [SerializeField] private float damageOverTimeDuration = 7f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ExplodeAfterDelay());
    }
    private IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(destroyTime);
        DamageEnemies();
        if (damageOverTimeDuration > 0)
        {
            gameObject.GetComponentInChildren<Renderer>().enabled = false;
        }
        
    }
    private IEnumerator ApplyDamageOverTime(NewEnemyHealth enemyHealth, float damage)
    {
        Debug.Log("ApplyDamageOverTime started");
        float timeElapsed = 0f;
        float damageInterval = damage / damageOverTimeDuration;

        while (timeElapsed < damageOverTimeDuration)
        {
            Debug.Log("Applying damage: " + damageInterval);
            enemyHealth.EnemyTakeDamage(damageInterval);
            yield return new WaitForSeconds(1f);
            timeElapsed++;
        }
        Destroy(gameObject);
    }
    void DamageEnemies()
    {
        int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");

        Collider[] colliders = Physics.OverlapSphere(transform.position, burstRadius, enemyLayerMask);

        float damage = finalSkillValue;
        Debug.Log("FinalSkillValue: " + damage);
        foreach (Collider collider in colliders)
        {
            NewEnemyHealth enemyHealth = collider.gameObject.GetComponent<NewEnemyHealth>();
            if (enemyHealth != null)
            {
                StartCoroutine(ApplyDamageOverTime(enemyHealth, damage));
                Debug.Log("Death Blossom hit!");
            }
        }
    }
}

