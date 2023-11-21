using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class Blitz : Skill
{
    //Skill specific fields
    [SerializeField] private float maxDistance = 5f;

    public override void DoSkill()
    { 
        DoDash();
        DamageEnemies();
    }

    void DoDash()
    {
        PlayerController playerController = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerController>();
        finalSkillCooldown = 8;
        playerController.StartCoroutine("Dodging");
        playerController.StartCoroutine("IFrames");
    }

    // void DamageEnemies()
    // {
    //     PlayerController playerController = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerController>();

    //     NewEnemyHealth enemyHealth = GameObject.FindWithTag("Enemy").GetComponent<NewEnemyHealth>();

    //     int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");
    //     RaycastHit hit;
    //     if (Physics.Raycast(transform.position, playerController.transform.forward, out hit, maxDistance, enemyLayerMask))
    //     {
    //         enemyHealth.EnemyTakeDamage(finalSkillCooldown);
    //         finalSkillCooldown *= 0.5f;
    //         Debug.DrawRay(transform.position, playerController.transform.forward * hit.distance, Color.white);
    //         Debug.Log("Did Hit");
    //     }
        
    // }

    void DamageEnemies()
    {
        int enemyLayerMask = 1 << LayerMask.NameToLayer("Enemy");

        Collider[] colliders = Physics.OverlapSphere(transform.position, maxDistance, enemyLayerMask);
        
        float damage = finalSkillValue;
        foreach (Collider collider in colliders)
        {
            NewEnemyHealth enemyHealth = collider.gameObject.GetComponent<NewEnemyHealth>();
            enemyHealth.EnemyTakeDamage(damage);
            finalSkillCooldown *= 0.5f;
        }
    }
}
