using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAtt : AttributeBase
{
    public override void Initialize(){
        if(stats == null || hit == null){return;}
        attName = "Light";
        attDesc = "\n15% faster attacks";
        stats.wpnName = attName + " " + stats.wpnName;
        interact.attributeDescription = attDesc;

        stats.wpnAttackSpeedModifier *= 1.15f;
    }
}
