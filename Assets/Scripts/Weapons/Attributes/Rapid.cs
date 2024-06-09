using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rapid : AttributeBase
{
    public override void Initialize(){
        attName = "Rapid";
        attDesc = "Double attack speed, half damage";
        if(stats == null || hit == null){return;}

        stats.wpnAttackSpeedModifier *= 2;
        stats.secondsTilHitstopSpeedup = 0;
        stats.statNums.advDamage.AddModifier(new StatModifier(-0.5f, StatModType.PercentAdd, this));
    }
}
