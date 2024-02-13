using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeathBlossomPlant : DeathBlossom
{
    [SerializeField] private float destroyTime = 2f;
    [SerializeField] private float burstRadius = 10f;
    [SerializeField] private float damageOverTimeDuration = 7f;
    [SerializeField] private int particleSpacing = 36;
    [SerializeField] private float particleHeight = 0f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ExplodeAfterDelay());
    }
    private IEnumerator ExplodeAfterDelay()
    {
        yield return new WaitForSeconds(destroyTime);
        DamageEnemies();
        DeathBlossomParticles();
        if (damageOverTimeDuration > 0)
        {
            gameObject.GetComponentInChildren<Renderer>().enabled = false;
        }

    }
    private IEnumerator ApplyDamageOverTime(EnemyHealth enemyHealth, float damage)
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
        SoundEffectManager.Instance.PlaySound("Explosion", transform.position);

        int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");

        Collider[] colliders = Physics.OverlapSphere(transform.position, burstRadius, enemyLayerMask);

        float damage = finalSkillValue;
        Debug.Log("FinalSkillValue: " + damage);
        foreach (Collider collider in colliders)
        {
            EnemyHealth enemyHealth = collider.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                StartCoroutine(ApplyDamageOverTime(enemyHealth, damage));
                Debug.Log("Death Blossom hit!");
            }
        }
    }
    void DeathBlossomParticles()
    {
        int particlesPerCircle = 360 / particleSpacing;

        int currentSmallSpacing = 0;
        for (int i = 0; i < particlesPerCircle; i++)
        {
            float smallX = Mathf.Cos(Mathf.Deg2Rad * currentSmallSpacing) * burstRadius;
            float smallZ = Mathf.Sin(Mathf.Deg2Rad * currentSmallSpacing) * burstRadius;

            InstantiateParticles(smallX, smallZ);
            currentSmallSpacing += particleSpacing;
        }
    }
    void InstantiateParticles(float x, float z)
    {
        Vector3 circlePosition = new Vector3(x, particleHeight, z);
        Vector3 spawnPosition = transform.position + circlePosition;
        ParticleManager.Instance.SpawnParticles("EruptionParticles", spawnPosition, Quaternion.LookRotation(Vector3.up, Vector3.up));
    }
}


