using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vigorous : AttributeBase
{
    public override void Initialize(){
        speedAmount = 3;
        vitalityAmount = 3;
        
        attName = "Vigorous";
        if(stats == null || hit == null){return;}
        stats.wpnName = attName + " " + stats.wpnName;
    }

    public override void Equipped(){
        characterStats.AddStat("Speed", speedAmount);
        characterStats.AddStat("Vitality", vitalityAmount);
    }

    public override void Unequipped(){
        characterStats.AddStat("Speed", -speedAmount);
        characterStats.AddStat("Vitality", -vitalityAmount);
    }
}
