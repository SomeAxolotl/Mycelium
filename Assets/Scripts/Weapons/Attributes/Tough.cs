using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tough : AttributeBase
{
    public override void Initialize(){
        vitalityAmount = 5;
        
        attName = "Tough";
        if(stats == null || hit == null){return;}
        stats.wpnName = attName + " " + stats.wpnName;
    }

    public override void Equipped(){
        characterStats.AddStat("Vitality", vitalityAmount);
    }

    public override void Unequipped(){
        characterStats.AddStat("Vitality", -vitalityAmount);
    }
}
