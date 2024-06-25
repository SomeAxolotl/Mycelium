using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armored : EnemyAttributeBase
{
    private EnemyHealth enemyHealth;
    private float damageReduction = 0.10f; // 10% damage reduction

    public override void Initialize()
    {
        enemyHealth = GetComponent<EnemyHealth>();
        if (enemyHealth != null && enemyHealth.isMiniBoss)
        {
            Debug.Log("Applying Armored attribute to miniboss: " + enemyHealth.miniBossName);
            enemyHealth.AddAttributePrefix("Armored"); // Add the attribute prefix to the miniboss name
        }
    }

    public float ApplyDamageReduction(float damage)
    {
       return damage * (1 - damageReduction);
    }
}
