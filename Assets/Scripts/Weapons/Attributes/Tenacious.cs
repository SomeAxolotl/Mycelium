using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tenacious : AttributeBase
{
    public override void Initialize(){
        if(stats == null || hit == null){return;}
        attName = "Tenacious";
        attDesc = "\n<sprite="+2+"> +3 <sprite="+3+"> +3";
        stats.wpnName = attName + " " + stats.wpnName;
        interact.attributeDescription = attDesc;
    }

    public override void Equipped(){
        characterStats.AddStat("Sentience", 3);
        characterStats.AddStat("Vitality", 3);
    }

    public override void Unequipped(){
        characterStats.AddStat("Sentience", -3);
        characterStats.AddStat("Vitality", -3);
    }
}
