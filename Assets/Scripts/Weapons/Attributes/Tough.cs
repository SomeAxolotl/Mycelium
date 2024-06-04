using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tough : AttributeBase
{
    public override void Initialize(){
        attName = "Tough";
        attDesc = "\n<sprite="+3+"> +5";
        if(stats == null || hit == null){return;}
        stats.wpnName = attName + " " + stats.wpnName;
        interact.attributeDescription = attDesc;
    }

    public override void Equipped(){
        characterStats.AddStat("Vitality", 5);
    }

    public override void Unequipped(){
        characterStats.AddStat("Vitality", -5);
    }
}
