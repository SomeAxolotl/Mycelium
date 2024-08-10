using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Armored : EnemyAttributeBase
{
    private float damageReduction = 0.20f; // Damage reduction percentage
    private GameObject armoredParticles;
    protected override void OnInitialize()
    {
        armoredParticles = transform.Find("ArmoredParticles").gameObject;
        armoredParticles.SetActive(true);
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
