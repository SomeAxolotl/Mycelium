using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tough : AttributeBase
{
    public override void Initialize(){
        vitalityAmount = 5;
        
        attName = "Tough";
    }

    public override void Equipped(){
        characterStats.AddStat("Vitality", vitalityAmount);
    }

    public override void Unequipped(){
        characterStats.AddStat("Vitality", -vitalityAmount);
    }
}
