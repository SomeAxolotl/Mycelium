using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strong : AttributeBase
{
    public override void Initialize(){
        attName = "Strong";
        attDesc = "\n<sprite="+0+"> +5";
        if(stats == null || hit == null){return;}
        stats.wpnName = attName + " " + stats.wpnName;
        interact.attributeDescription = attDesc;
    }

    public override void Equipped(){
        characterStats.AddStat("Primal", 5);
    }

    public override void Unequipped(){
        characterStats.AddStat("Primal", -5);
    }
}
