using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agile : AttributeBase
{
    public override void Initialize(){
        attName = "Agile";
        attDesc = "\n<sprite="+1+"> +5";
        if(stats == null || hit == null){return;}
        stats.wpnName = attName + " " + stats.wpnName;
        interact.attributeDescription = attDesc;
    }

    public override void Equipped(){
        characterStats.AddStat("Speed", 5);
    }

    public override void Unequipped(){
        characterStats.AddStat("Speed", -5);
    }
}
