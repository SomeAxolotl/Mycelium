using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sharp : AttributeBase
{
    public override void Initialize(){
        attName = "Sharp";
        attDesc = "\n<sprite="+0+"> +3 <sprite="+2+"> +3";
        if(stats == null || hit == null){return;}
        stats.wpnName = attName + " " + stats.wpnName;
        interact.attributeDescription = attDesc;
    }

    public override void Equipped(){
        characterStats.AddStat("Primal", 3);
        characterStats.AddStat("Sentience", 3);
    }

    public override void Unequipped(){
        characterStats.AddStat("Primal", -3);
        characterStats.AddStat("Sentience", -3);
    }
}
