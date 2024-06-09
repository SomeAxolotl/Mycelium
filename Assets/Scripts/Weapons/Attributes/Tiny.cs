using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiny : AttributeBase
{
    private float newSize = -0.25f;

    public override void Initialize(){
        attName = "Tiny";
        attDesc = "25% smaller, +50% attack speed";

        if(stats == null || hit == null){return;}
        stats.statNums.advSize.AddModifier(new StatModifier(newSize, StatModType.PercentAdd, this));
        //Because Initialize happens OnEnable -Before Start- we need to check if this is happening before or after initialization
        if(stats.O_startSize != Vector3.zero){
            stats.AdjustSize();
        }

        stats.wpnAttackSpeedModifier *= 1.5f;
        //Why was this also doing more damage RONALD
        //stats.wpnMult *= 1.5f;
    }
}
