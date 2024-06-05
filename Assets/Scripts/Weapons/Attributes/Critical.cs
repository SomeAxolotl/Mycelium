using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Critical : AttributeBase
{
    public override void Initialize(){
        attName = "Critical";
        attDesc = "15% chance to deal double damage";
        if(stats == null || hit == null){return;}
        stats.wpnName = attName + " " + stats.wpnName;
    }

    public override void Hit(GameObject target, float damage){
        //15% to double damage dealt
        if(Random.Range(0, 100) <= 15){
            hit.dmgDealt *= 2;
        }
    }
}