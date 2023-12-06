using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombify : Skill
{
    [SerializeField] private float destroyTime = 5f;
    [SerializeField] private float burstRadius = 10f;
    //Skill specific fields

    public override void DoSkill()
    {
        ZombifyNearestEnemy();
        EndSkill();
    }
    void ZombifyNearestEnemy()
    {
        
        GameObject nearestEnemy = FindNearestEnemy();
        if (nearestEnemy != null)
        {
            ChangeLayerToEnemy(nearestEnemy);
            StartCoroutine(ExplodeAfterDelay());
        }
    }
    GameObject FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        GameObject nearestEnemy = null;
        float nearestDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            if (enemy != gameObject)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestEnemy = enemy;
                }
            }
        }

        return nearestEnemy;
    }
    void ChangeLayerToEnemy(GameObject enemy)
    {
        EnemyNavigation enemyNavigation = enemy.GetComponent<EnemyNavigation>();
        if (enemyNavigation != null)
        {
            int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");
            enemyNavigation.playerLayer = enemyLayerMask;
            
        }
    }

    private IEnumerator ExplodeAfterDelay()
    {
        NavMeshAgent navMeshAgent = gameObject.GetComponent<NavMeshAgent>();
        if (navMeshAgent != null)
        {
            GameObject nearestEnemy = FindNearestEnemy();

            if (nearestEnemy != null)
            {
                // Set the destination of the NavMeshAgent to the position of the nearest enemy
                navMeshAgent.SetDestination(nearestEnemy.transform.position);
            }
        }
        yield return new WaitForSeconds(destroyTime);
        DamageEnemies();
    }
    void DamageEnemies()
    {
        int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");
        int playerLayerMask = 2 << LayerMask.NameToLayer("Player");

        Collider[] colliders = Physics.OverlapSphere(transform.position, burstRadius, enemyLayerMask);
        Collider[] pcolliders = Physics.OverlapSphere(transform.position, burstRadius, playerLayerMask);
        float damage = finalSkillValue * 5;
        foreach (Collider collider in colliders)
        {
            EnemyHealth enemyHealth = collider.gameObject.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                if (collider.gameObject != gameObject)
                {
                    enemyHealth.EnemyTakeDamage(damage);
                    Debug.Log("ZOMBIE EXPLODE!!!" + damage);
                }
            }
            
        }
        foreach (Collider collider in pcolliders)
        {
            PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                if (collider.gameObject != gameObject)
                {
                    playerHealth.PlayerTakeDamage(damage);
                    Debug.Log("ZOMBIE EXPLODE!!!" + damage);
                }
            }

        }
    }
}
