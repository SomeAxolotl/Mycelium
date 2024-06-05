using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sharp : AttributeBase
{
    public override void Initialize(){
        primalAmount = 3;
        sentienceAmount = 3;

        attName = "Sharp";
        if(stats == null || hit == null){return;}
        stats.wpnName = attName + " " + stats.wpnName;
    }

    public override void Equipped(){
        characterStats.AddStat("Primal", primalAmount);
        characterStats.AddStat("Sentience", sentienceAmount);
    }

    public override void Unequipped(){
        characterStats.AddStat("Primal", -primalAmount);
        characterStats.AddStat("Sentience", -sentienceAmount);
    }
}
