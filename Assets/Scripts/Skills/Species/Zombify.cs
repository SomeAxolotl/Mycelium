using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombify : Skill
{
    [SerializeField] private float destroyTime = 5f;
    [SerializeField] private float burstRadius = 10f;
    [SerializeField] private float zombifyRange = 7f;
    //[SerializeField] private int particleSpacing = 36;
    //[SerializeField] private float particleHeight = 0f;
    public Material zombifyMaterial;
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
        float nearestDistance = zombifyRange;

        foreach (GameObject enemy in enemies)
        {
            if (enemy != gameObject)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance <= nearestDistance)
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
        RangedEnemyShoot rangedEnemyShoot = enemy.GetComponent<RangedEnemyShoot>();
        MeleeEnemyAttack meleeEnemyAttack = enemy.GetComponent<MeleeEnemyAttack>();
        NavMeshAgent navMeshAgent = enemy.GetComponent<NavMeshAgent>();
        if (enemyNavigation != null)
        {
            int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");
            enemyNavigation.playerLayer = enemyLayerMask;
            Renderer enemyRenderer = enemy.GetComponent<Renderer>();
            if (enemyRenderer == null)
            {
                enemyRenderer = enemy.AddComponent<MeshRenderer>();
            }
            enemyRenderer.material = zombifyMaterial;
            navMeshAgent.stoppingDistance = 2.5f;
        }
        if (enemyNavigation.playerSeen == false)
        {
            transform.position += transform.forward * 5f * Time.deltaTime;
        }
        if (rangedEnemyShoot != null)
        {
            int nothingLayerMask = 2 << LayerMask.NameToLayer("Nothing");
            //rangedEnemyShoot.playerLayer = nothingLayerMask;
        }
        if (meleeEnemyAttack != null)
        {
            int nothingLayerMask = 2 << LayerMask.NameToLayer("Nothing");
            //meleeEnemyAttack.playerLayer = nothingLayerMask;
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
        //ZombifyParticles();
    }
    void DamageEnemies()
    {
        int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");

        Collider[] colliders = Physics.OverlapSphere(transform.position, burstRadius, enemyLayerMask);
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
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, burstRadius);
    }
    /*void ZombifyParticles()
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
    }*/
}

