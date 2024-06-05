using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampiric : AttributeBase
{
    PlayerHealth health;

    public override void Initialize(){
        attName = "Vampiric";
        attDesc = "Heal 15% of damage dealt";
        if(stats == null || hit == null){return;}
        stats.wpnName = attName + " " + stats.wpnName;
    }

    public override void Equipped(){
        base.Equipped();
        health = playerParent.GetComponent<PlayerHealth>();
    }

    public override void Hit(GameObject target, float damage){
        if(health != null){
            health.SpawnHealingOrb(target.transform.position, (damage * 0.15f));
        }
    }
}
