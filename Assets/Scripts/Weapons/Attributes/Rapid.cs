using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rapid : AttributeBase
{
    public override void Initialize(){
        if(stats == null || hit == null){return;}
        attName = "Rapid";
        attDesc = "\nDouble attack speed, half damage";
        stats.wpnName = attName + " " + stats.wpnName;
        interact.attributeDescription = attDesc;

        stats.wpnAttackSpeedModifier *= 2;
        stats.secondsTilHitstopSpeedup = 0;
        stats.wpnMult /= 2;
    }
}
