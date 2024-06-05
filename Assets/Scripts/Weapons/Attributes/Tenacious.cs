using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tenacious : AttributeBase
{
    public override void Initialize(){
        sentienceAmount = 3;
        vitalityAmount = 3;
        
        attName = "Tenacious";
        if(stats == null || hit == null){return;}
        stats.wpnName = attName + " " + stats.wpnName;
    }

    public override void Equipped(){
        characterStats.AddStat("Sentience", sentienceAmount);
        characterStats.AddStat("Vitality", vitalityAmount);
    }

    public override void Unequipped(){
        characterStats.AddStat("Sentience", -sentienceAmount);
        characterStats.AddStat("Vitality", -vitalityAmount);
    }
}
