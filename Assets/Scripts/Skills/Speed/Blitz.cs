using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Blitz : Skill
{
    //Skill specific fields
    [SerializeField] private float maxDistance = 5f;
    [SerializeField] private GameObject enemy;

    public override void DoSkill()
    { 
        DoDash();
        DamageEnemies();
        EndSkill();
    }

    void DoDash()
    {
        finalSkillCooldown = 8;
        playerController.StartCoroutine("Dodging");
        playerController.StartCoroutine("IFrames");
    }

    void DamageEnemies()
    {
        int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");
        RaycastHit hit;
        if (Physics.Raycast(transform.position, playerController.transform.forward, out hit, maxDistance, enemyLayerMask))
        {
            // enemyHealth.EnemyTakeDamage(finalSkillCooldown);
            enemy = GameObject.FindWithTag("Enemy");
            enemy.GetComponent<NewEnemyHealth>().EnemyTakeDamage(finalSkillValue);
            finalSkillCooldown *= 0.5f;
        }
    }

    // void DamageEnemies()
    // {
    //     int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");

    //     Collider[] colliders = Physics.OverlapSphere(transform.position, maxDistance, enemyLayerMask);
        
    //     float damage = finalSkillValue;
    //     foreach (Collider collider in colliders)
    //     {
    //         NewEnemyHealth enemyHealth = collider.gameObject.GetComponent<NewEnemyHealth>();
    //         enemyHealth.EnemyTakeDamage(damage);
    //         finalSkillCooldown *= 0.5f;
    //     }
    // }
}