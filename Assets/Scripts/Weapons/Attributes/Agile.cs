using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agile : AttributeBase
{
    public override void Initialize(){
        speedAmount = 5;
        
        attName = "Agile";
        if(stats == null || hit == null){return;}
        stats.wpnName = attName + " " + stats.wpnName;
    }

    public override void Equipped(){
        characterStats.AddStat("Speed", speedAmount);
    }

    public override void Unequipped(){
        characterStats.AddStat("Speed", -speedAmount);
    }
}
