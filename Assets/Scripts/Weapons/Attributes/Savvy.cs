using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Savvy : AttributeBase
{
    public override void Initialize(){
        attName = "Savvy";
        attDesc = "\n<sprite="+2+"> +3 <sprite="+1+"> +3";
        if(stats == null || hit == null){return;}
        stats.wpnName = attName + " " + stats.wpnName;
        interact.attributeDescription = attDesc;
    }

    public override void Equipped(){
        characterStats.AddStat("Sentience", 3);
        characterStats.AddStat("Speed", 3);
    }

    public override void Unequipped(){
        characterStats.AddStat("Sentience", -3);
        characterStats.AddStat("Speed", -3);
    }
}
