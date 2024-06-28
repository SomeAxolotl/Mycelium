using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parasitic : EnemyAttributeBase
{
    protected override void OnInitialize()
    {
        // Initialization logic specific to Parasitic attribute
    }

    public float GetHealAmount(float damageDealt)
    {
        return damageDealt * 0.2f; // Heal for 20% of the damage dealt
    }
}
