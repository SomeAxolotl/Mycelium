using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heavy : AttributeBase
{
    public override void Initialize(){
        attName = "Heavy";
        attDesc = "25% slower, 50% stronger";
        
        if(stats == null || hit == null){return;}
        stats.wpnAttackSpeedModifier *= 0.75f;
        //stats.secondsTilHitstopSpeedup = 0;
        stats.statNums.advDamage.AddModifier(new StatModifier(0.5f, StatModType.PercentAdd, this));
    }
}
