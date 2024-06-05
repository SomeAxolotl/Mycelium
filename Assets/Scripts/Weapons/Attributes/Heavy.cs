using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heavy : AttributeBase
{
    public override void Initialize(){
        attName = "Heavy";
        attDesc = "25% slower, 50% stronger";
        if(stats == null || hit == null){return;}
        stats.wpnName = attName + " " + stats.wpnName;

        stats.wpnAttackSpeedModifier *= 0.75f;
        //stats.secondsTilHitstopSpeedup = 0;
        stats.wpnMult *= 1.5f;
    }
}
