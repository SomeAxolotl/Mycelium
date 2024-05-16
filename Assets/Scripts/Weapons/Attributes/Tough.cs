using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tough : AttributeBase
{
    public override void Initialize(){
        if(stats == null || hit == null){return;}
        attName = "Tough";
        attDesc = "\n<sprite="+3+"> +5";
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
