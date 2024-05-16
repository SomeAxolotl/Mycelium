using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vigorous : AttributeBase
{
    public override void Initialize(){
        if(stats == null || hit == null){return;}
        attName = "Vigorous";
        attDesc = "\n<sprite="+1+"> +3 <sprite="+3+"> +3";
        stats.wpnName = attName + " " + stats.wpnName;
        interact.attributeDescription = attDesc;
    }

    public override void Equipped(){
        characterStats.AddStat("Speed", 3);
        characterStats.AddStat("Vitality", 3);
    }

    public override void Unequipped(){
        characterStats.AddStat("Speed", -3);
        characterStats.AddStat("Vitality", -3);
    }
}
