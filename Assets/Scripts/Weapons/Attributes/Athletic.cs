using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Athletic : AttributeBase
{
    public override void Initialize(){
        primalAmount = 3;
        speedAmount = 3;
        
        attName = "Athletic";
    }

    public override void Equipped(){
        characterStats.AddStat("Primal", primalAmount);
        characterStats.AddStat("Speed", speedAmount);
    }

    public override void Unequipped(){
        characterStats.AddStat("Primal", -primalAmount);
        characterStats.AddStat("Speed", -speedAmount);
    }
}
