using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAtt : AttributeBase
{
    public override void Initialize(){
        attName = "Light";
        attDesc = "\n15% faster attacks";
        if(stats == null || hit == null){return;}
        stats.wpnName = attName + " " + stats.wpnName;
        interact.attributeDescription = attDesc;

        stats.wpnAttackSpeedModifier *= 1.15f;
    }
}
