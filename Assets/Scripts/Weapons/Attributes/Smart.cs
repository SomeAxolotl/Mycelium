using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smart : AttributeBase
{
    public override void Initialize(){
        attName = "Smart";
        attDesc = "\n<sprite="+2+"> +5";
        if(stats == null || hit == null){return;}
        stats.wpnName = attName + " " + stats.wpnName;
        interact.attributeDescription = attDesc;
    }

    public override void Equipped(){
        characterStats.AddStat("Sentience", 5);
    }

    public override void Unequipped(){
        characterStats.AddStat("Sentience", -5);
    }
}
