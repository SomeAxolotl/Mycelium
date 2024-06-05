using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smart : AttributeBase
{
    public override void Initialize(){
        sentienceAmount = 5;
        
        attName = "Smart";
    }

    public override void Equipped(){
        characterStats.AddStat("Sentience", sentienceAmount);
    }

    public override void Unequipped(){
        characterStats.AddStat("Sentience", -sentienceAmount);
    }
}
