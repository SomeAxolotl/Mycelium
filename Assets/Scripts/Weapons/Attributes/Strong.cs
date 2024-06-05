using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strong : AttributeBase
{
    public override void Initialize(){
        primalAmount = 5;
        
        attName = "Strong";
    }

    public override void Equipped(){
        characterStats.AddStat("Primal", primalAmount);
    }

    public override void Unequipped(){
        characterStats.AddStat("Primal", -primalAmount);
    }
}
