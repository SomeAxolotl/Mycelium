using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Critical : AttributeBase
{
    public override void Initialize(){
        if(stats == null || hit == null){return;}
        attName = "Critical";
        attDesc = "\n15% chance to deal double damage";
        stats.wpnName = attName + " " + stats.wpnName;
        interact.attributeDescription = attDesc;
    }

    public override void Hit(GameObject target, float damage){
        //15% to double damage dealt
        if(Random.Range(0, 100) <= 15){
            hit.dmgDealt *= 2;
        }
    }
}