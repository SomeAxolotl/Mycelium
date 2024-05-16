using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sticky : AttributeBase
{
    public override void Initialize(){
        if(stats == null || hit == null){return;}
        attName = "Sticky";
        attDesc = "\nApply 50% slow for 2 seconds";
        stats.wpnName = attName + " " + stats.wpnName;
        interact.attributeDescription = attDesc;
    }

    public override void Hit(GameObject target, float damage){
        SpeedChange speedChangeEffect = target.AddComponent<SpeedChange>();
        //Deals 20% of weapon damage over 5 seconds
        speedChangeEffect.InitializeSpeedChange(2, -50);
    }
}
