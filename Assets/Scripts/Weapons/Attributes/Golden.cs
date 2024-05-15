using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Golden : AttributeBase
{
    GameObject player;
    CharacterStats characterStats;

    HUDStats hudStats;

    void Awake(){
        player = GameObject.FindWithTag("currentPlayer");
        characterStats = player.GetComponent<CharacterStats>();
        hudStats = GameObject.Find("HUD").GetComponent<HUDStats>();
    }

    public override void Initialize(){
        if(stats == null || hit == null){return;}
        attName = "Golden";
        attDesc = "\n<sprite="+0+"> +1 <sprite="+2+"> +1 <sprite="+1+"> +1 <sprite="+3+"> +1";
        stats.wpnName = attName + " " + stats.wpnName;
        interact.attributeDescription = attDesc;
    }

    public override void Equipped(){
        characterStats.AddStat("Primal", 1);
        characterStats.AddStat("Sentience", 1);
        characterStats.AddStat("Speed", 1);
        characterStats.AddStat("Vitality", 1);
    }

    public override void Unequipped(){
        characterStats.AddStat("Primal", -1);
        characterStats.AddStat("Sentience", -1);
        characterStats.AddStat("Speed", -1);
        characterStats.AddStat("Vitality", -1);
    }
}
