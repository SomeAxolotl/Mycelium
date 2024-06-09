using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Massive : AttributeBase
{
    private float newSize = 0.5f;

    public override void Initialize(){
        attName = "Massive";
        attDesc = "50% larger";
        
        if(stats == null || hit == null){return;}

        stats.statNums.advSize.AddModifier(new StatModifier(newSize, StatModType.PercentAdd, this));
        //Because Initialize happens OnEnable -Before Start- we need to check if this is happening before or after initialization
        if(stats.O_startSize != Vector3.zero){
            stats.AdjustSize();
        }
    }
}
