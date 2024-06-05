using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncy : AttributeBase
{
    public override void Initialize(){
        attName = "Bouncy";
        attDesc = "Increase knockback by 50%";
        if(stats == null || hit == null){return;}
        stats.wpnName = attName + " " + stats.wpnName;

        stats.wpnKnockback = stats.wpnKnockback * 1.5f;
    }
}
