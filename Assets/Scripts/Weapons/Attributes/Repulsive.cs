using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Repulsive : AttributeBase
{
    public override void Initialize(){
        attName = "Repulsive";
        attDesc = "150% damage converted to Poison";
        if(stats == null || hit == null){return;}
        stats.wpnName = attName + " " + stats.wpnName;

        //Sets damage mult to 0
        //stats.wpnMult = 0;
    }

    public override void Hit(GameObject target, float damage){
        //Makes sure hit does not damage
        hit.dmgDealt = 0;
        Poison poisonEffect = target.AddComponent<Poison>();
        //Deals 150% of weapon damage over 5 seconds
        poisonEffect.PoisonStats((damage / 5) * 1.5f);
    }
}