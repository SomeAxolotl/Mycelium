using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agile : AttributeBase
{
    public override void Initialize(){
        if(stats == null || hit == null){return;}
        attName = "Agile";
        attDesc = "\n<sprite="+1+"> +5";
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
