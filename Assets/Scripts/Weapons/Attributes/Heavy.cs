using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heavy : AttributeBase
{
    public override void Initialize(){
        attName = "Heavy";
        attDesc = "\n25% slower, 50% stronger";
        if(stats == null || hit == null){return;}
        stats.wpnName = attName + " " + stats.wpnName;
        interact.attributeDescription = attDesc;

        stats.wpnAttackSpeedModifier *= 0.75f;
        //stats.secondsTilHitstopSpeedup = 0;
        stats.wpnMult *= 1.5f;
    }
}
