using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beefy : AttributeBase
{
    public override void Initialize(){
        primalAmount = 3;
        vitalityAmount = 3;
        
        attName = "Beefy";
        if(stats == null || hit == null){return;}
        stats.wpnName = attName + " " + stats.wpnName;
    }

    public override void Equipped(){
        characterStats.AddStat("Primal", primalAmount);
        characterStats.AddStat("Vitality", vitalityAmount);
    }

    public override void Unequipped(){
        characterStats.AddStat("Primal", -primalAmount);
        characterStats.AddStat("Vitality", -vitalityAmount);
    }
}
