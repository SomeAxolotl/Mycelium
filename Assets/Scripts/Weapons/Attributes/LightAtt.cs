using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightAtt : AttributeBase
{
    public override void Initialize(){
        attName = "Light";
        attDesc = "15% faster attacks";

        if(stats == null || hit == null){return;}
        stats.wpnAttackSpeedModifier *= 1.15f;
    }
}
