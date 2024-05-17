using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vampiric : AttributeBase
{
    PlayerHealth health;

    public override void Initialize(){
        if(stats == null || hit == null){return;}
        attName = "Vampiric";
        attDesc = "\nHeal 15% of damage dealt";
        stats.wpnName = attName + " " + stats.wpnName;
        interact.attributeDescription = attDesc;
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
