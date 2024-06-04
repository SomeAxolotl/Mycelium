using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sticky : AttributeBase
{
    public override void Initialize(){
        attName = "Sticky";
        attDesc = "\nApply 50% slow for 2 seconds";
        if(stats == null || hit == null){return;}
        stats.wpnName = attName + " " + stats.wpnName;
        interact.attributeDescription = attDesc;
    }

    public override void Hit(GameObject target, float damage){
        SpeedChange speedChangeEffect = target.AddComponent<SpeedChange>();
        speedChangeEffect.InitializeSpeedChange(2, -50);
    }
}
