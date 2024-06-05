using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nasty : AttributeBase
{
    public override void Initialize(){
        attName = "Nasty";
        attDesc = "Apply 20% daamage as a poison";
        if(stats == null || hit == null){return;}
        stats.wpnName = attName + " " + stats.wpnName;
    }

    public override void Hit(GameObject target, float damage){
        Poison poisonEffect = target.AddComponent<Poison>();
        //Deals 20% of weapon damage over 5 seconds
        poisonEffect.PoisonStats(damage / 25f);
    }
}
