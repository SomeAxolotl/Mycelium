using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rapid : AttributeBase
{
    public override void Initialize(){
        attName = "Rapid";
        attDesc = "Double attack speed, half damage";
        if(stats == null || hit == null){return;}
        stats.wpnName = attName + " " + stats.wpnName;

        stats.wpnAttackSpeedModifier *= 2;
        stats.secondsTilHitstopSpeedup = 0;
        stats.wpnMult /= 2;
    }
}
