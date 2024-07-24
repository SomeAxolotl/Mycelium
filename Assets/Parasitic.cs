using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parasitic : EnemyAttributeBase
{
    private GameObject parasiticParticles;
    protected override void OnInitialize()
    {
        parasiticParticles = transform.Find("ParasiticParticles").gameObject;
        parasiticParticles.SetActive(true);
    }

    public float GetHealAmount(float damageDealt)
    {
        return damageDealt * 0.2f; // Heal for 20% of the damage dealt
    }
}
