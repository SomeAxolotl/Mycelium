using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Athletic : AttributeBase
{
    public override void Initialize(){
        if(stats == null || hit == null){return;}
        attName = "Athletic";
        attDesc = "\n<sprite="+0+"> +3 <sprite="+1+"> +3";
        stats.wpnName = attName + " " + stats.wpnName;
        interact.attributeDescription = attDesc;
    }

    public override void Equipped(){
        characterStats.AddStat("Primal", 3);
        characterStats.AddStat("Speed", 3);
    }

    public override void Unequipped(){
        characterStats.AddStat("Primal", -3);
        characterStats.AddStat("Speed", -3);
    }
}
