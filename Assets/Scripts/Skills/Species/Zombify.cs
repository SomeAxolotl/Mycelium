using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombify : Skill
{
    [SerializeField] private float zombifyRange = 7f;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private float reducedCooldownPercentZ = .7f; 
    private Collider[] enemyColliders;
    //Skill specific fields

    public override void DoSkill()
    {
        if(GetClosestEnemy() != null)
        {
            DoZombify(GetClosestEnemy()); 
        }
        else
        {
            StartCooldown(GetFinalCooldown() * reducedCooldownPercentZ);
        }
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
            Madness madness = enemy.AddComponent<Madness>();
            madness.ApplyMadness(finalSkillValue, 0.5f);

            /*
            if(enemy.GetComponent<CrabAttack>() != null)
            {
                enemy.GetComponent<CrabAttack>().StopAttack();
            }
            if (enemy.GetComponent<MushyAttack>() != null)
            {
                enemy.GetComponent<MushyAttack>().StopAttack();
            }
            enemy.GetComponent<EnemyAttack>().CancelAttack();
            enemy.GetComponent<EnemyAttack>().enabled = false;
            enemy.GetComponent<NavEnabler>().enabled = false;
            enemy.GetComponent<ReworkedEnemyNavigation>().enabled = false;
            enemy.GetComponent<ZombifiedMovement>().enabled = true;
            enemy.GetComponent<ZombifiedMovement>().explosionDamage = finalSkillValue;
            */
        }
    }
}

