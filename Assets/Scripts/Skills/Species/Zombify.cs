using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombify : Skill
{
    [SerializeField] private float zombifyRange = 7f;
    [SerializeField] private LayerMask enemyLayer;
    private Collider[] enemyColliders;
    //Skill specific fields

    public override void DoSkill()
    {
        DoZombify(GetClosestEnemy());
        EndSkill();
    }
    GameObject GetClosestEnemy()
    {
        enemyColliders = Physics.OverlapSphere(player.transform.position, zombifyRange, enemyLayer);
        GameObject closestEnemy = null;
        float closestDistance = zombifyRange;
        foreach (Collider collider in enemyColliders)
        {
            float distance = Vector3.Distance(player.transform.position, collider.transform.position);
            if (distance < closestDistance)
            {
                closestEnemy = collider.gameObject;
                closestDistance = distance;
            }
        }
        return closestEnemy;
    }
    void DoZombify(GameObject enemy)
    {
        if(enemy != null && enemy.GetComponent<EnemyAttack>() != null && enemy.GetComponent<NavEnabler>() != null && enemy.GetComponent<ReworkedEnemyNavigation>() != null && enemy.GetComponent<ZombifiedMovement>() != null)
        {
            enemy.GetComponent<EnemyAttack>().CancelAttack();
            enemy.GetComponent<EnemyAttack>().enabled = false;
            enemy.GetComponent<NavEnabler>().enabled = false;
            enemy.GetComponent<ReworkedEnemyNavigation>().enabled = false;
            enemy.GetComponent<ZombifiedMovement>().enabled = true;
            enemy.GetComponent<ZombifiedMovement>().explosionDamage = finalSkillValue;
        }
    }
}

