using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Savvy : AttributeBase
{
    public override void Initialize(){
        sentienceAmount = 3;
        speedAmount = 3;

        attName = "Savvy";
    }

    public override void Equipped(){
        characterStats.AddStat("Sentience", sentienceAmount);
        characterStats.AddStat("Speed", speedAmount);
    }

    public override void Unequipped(){
        characterStats.AddStat("Sentience", -sentienceAmount);
        characterStats.AddStat("Speed", -speedAmount);
    }
}
