using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncy : AttributeBase
{
    public override void Initialize(){
        if(stats == null || hit == null){return;}
        attName = "Bouncy";
        attDesc = "\nIncrease knockback by 50%";
        stats.wpnName = attName + " " + stats.wpnName;
        interact.attributeDescription = attDesc;

        stats.wpnKnockback = stats.wpnKnockback * 1.5f;
    }
}
