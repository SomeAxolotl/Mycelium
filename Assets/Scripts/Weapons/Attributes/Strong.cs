using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strong : AttributeBase
{
    public override void Initialize(){
        primalAmount = 5;
        
        attName = "Strong";
        if(stats == null || hit == null){return;}
        stats.wpnName = attName + " " + stats.wpnName;
    }

    public override void Equipped(){
        characterStats.AddStat("Primal", primalAmount);
    }

    public override void Unequipped(){
        characterStats.AddStat("Primal", -primalAmount);
    }
}
