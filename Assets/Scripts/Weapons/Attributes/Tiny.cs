using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tiny : AttributeBase
{
    private float newSize = 0.75f;

    public override void Initialize(){
        attName = "Tiny";
        attDesc = "\n25% smaller, +50% attack speed";
        if(stats == null || hit == null){return;}
        stats.wpnName = attName + " " + stats.wpnName;
        interact.attributeDescription = attDesc;

        Vector3 newScale = new Vector3(transform.localScale.x * newSize, transform.localScale.y * newSize, transform.localScale.z * newSize);
        transform.localScale = newScale;

        stats.wpnAttackSpeedModifier *= 1.5f;
        stats.wpnMult *= 1.5f;
    }
}
