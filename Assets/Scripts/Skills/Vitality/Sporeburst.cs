using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sporeburst : Skill
{
    [SerializeField] private GameObject sporeburstParticlePrefab;

    [SerializeField] private float percentageOfDamageHealed = 0.5f;
    [SerializeField] private float aoeRadius = 3f;
    [SerializeField] private int particleSpacing = 36;

    public override void DoSkill()
    {
        DamageEnemies();
        SporeburstParticles();
        SoundEffectManager.Instance.PlaySound("Smash", player.transform.position);
        EndSkill();
    }

    void DamageEnemies()
    {
        int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");

        Collider[] colliders = Physics.OverlapSphere(transform.position, aoeRadius, enemyLayerMask);
        
        float damage = finalSkillValue;
        foreach (Collider collider in colliders)
        {
            NewEnemyHealth enemyHealth = collider.gameObject.GetComponent<NewEnemyHealth>();
            enemyHealth.EnemyTakeDamage(damage);
            playerHealth.PlayerHeal(damage * percentageOfDamageHealed);
        }
    }

    void SporeburstParticles()
    {
        int particlesPerCircle = 360 / particleSpacing;
        
        int currentSpacing = 0;
        for (int i = 0; i < particlesPerCircle; i++)
        {
            float smallX = Mathf.Cos(Mathf.Deg2Rad * currentSpacing) * 0.1f;
            float smallZ = Mathf.Sin(Mathf.Deg2Rad * currentSpacing) * 0.1f;

            InstantiateParticles(smallX, smallZ);
            currentSpacing += particleSpacing;
        }
    }

    void InstantiateParticles(float x, float z)
    {
        Vector3 circlePosition = new Vector3(x, 0f, z);
        Vector3 spawnPosition = transform.position + circlePosition;

        Vector3 direction = (spawnPosition - transform.position).normalized;
        Quaternion rotation = Quaternion.LookRotation(direction);

        ParticleManager.Instance.SpawnParticles("SporeburstParticles", spawnPosition, rotation);
    }
}
