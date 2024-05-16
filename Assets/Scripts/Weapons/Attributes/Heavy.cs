using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heavy : AttributeBase
{
    public override void Initialize(){
        if(stats == null || hit == null){return;}
        attName = "Heavy";
        attDesc = "\n25% slower, 50% stronger";
        stats.wpnName = attName + " " + stats.wpnName;
        interact.attributeDescription = attDesc;

        stats.wpnAttackSpeedModifier *= 0.75f;
        //stats.secondsTilHitstopSpeedup = 0;
        stats.wpnMult *= 1.5f;
    }
}
