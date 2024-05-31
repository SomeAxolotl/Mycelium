using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eruption : Skill
{
    [SerializeField] private float sourSpotScalar = 0.5f;
    [SerializeField] private GameObject eruptionParticlePrefab;

    [SerializeField] private float smallRadius = 3f;
    [SerializeField] private float largeRadius = 6f;

    //[SerializeField] private int particleSpacing = 36;
    [SerializeField] private float particleHeight = 1f;

    public override void DoSkill()
    {
        DamageEnemies();
        //EruptionParticles();
        ParticleManager.Instance.SpawnParticles("EruptionParticles2", player.transform.position + new Vector3(0, particleHeight, 0), Quaternion.Euler(-90,0,0));
        SoundEffectManager.Instance.PlaySound("Explosion", player.transform, 0, 0.7f);
        EndSkill();
    }

    void DamageEnemies()
    {
        int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");

        Collider[] colliders = Physics.OverlapSphere(transform.position, largeRadius, enemyLayerMask);
        
        List<GameObject> enemies = new List<GameObject>();
        foreach (Collider collider in colliders)
        {
            float finalDamage = finalSkillValue;
            float distanceToCollider = Vector3.Distance(transform.position, collider.transform.position);

            if (distanceToCollider >= smallRadius)
            {
                finalDamage *= sourSpotScalar;
            }
            if(collider.gameObject.GetComponent<EnemyHealth>() != null && !enemies.Contains(collider.gameObject))
            {
                enemies.Add(collider.gameObject);
                EnemyHealth enemyHealth = collider.gameObject.GetComponent<EnemyHealth>();
                enemyHealth.EnemyTakeDamage(finalDamage);
            }
        }
    }

    /*void EruptionParticles()
    {
        int particlesPerCircle = 360 / particleSpacing;
        
        int currentSmalLSpacing = 0;
        for (int i = 0; i < particlesPerCircle; i++)
        {
            float smallX = Mathf.Cos(Mathf.Deg2Rad * currentSmalLSpacing) * smallRadius;
            float smallZ = Mathf.Sin(Mathf.Deg2Rad * currentSmalLSpacing) * smallRadius;

            InstantiateParticles(smallX, smallZ);
            currentSmalLSpacing += particleSpacing;
        }

        int curentLargeSpacing = 0;
        for (int i = 0; i < particlesPerCircle; i++)
        {
            float largeX = Mathf.Cos(Mathf.Deg2Rad * curentLargeSpacing) * largeRadius;
            float largeZ = Mathf.Sin(Mathf.Deg2Rad * curentLargeSpacing) * largeRadius;

            InstantiateParticles(largeX, largeZ);
            curentLargeSpacing += particleSpacing;
        }
    }

    void InstantiateParticles(float x, float z)
    {
        Vector3 circlePosition = new Vector3(x, particleHeight, z);
        Vector3 spawnPosition = transform.position + circlePosition;
        ParticleManager.Instance.SpawnParticles("EruptionParticles", spawnPosition, Quaternion.LookRotation(Vector3.up, Vector3.up));
    }*/
}
