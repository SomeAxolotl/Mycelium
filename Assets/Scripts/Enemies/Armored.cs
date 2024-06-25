using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armored : EnemyAttributeBase
{
    private float damageReduction = 0.10f; // Damage reduction percentage

    protected override void OnInitialize()
    {
        // Initialization logic specific to Armored attribute, if any
    }

    public float ApplyDamageReduction(float damage)
    {
        return damage * (1 - damageReduction);
    }

    public override void OnTakeDamage(float damage)
    {
        // Apply damage reduction
        float reducedDamage = ApplyDamageReduction(damage);
        GetComponent<EnemyHealth>().EnemyTakeDamage(reducedDamage);
    }
}
