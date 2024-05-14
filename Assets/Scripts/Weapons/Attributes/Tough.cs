using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tough : AttributeBase
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
        attName = "Tough";
        attDesc = "\n<sprite="+3+"> +6";
        stats.wpnName = attName + " " + stats.wpnName;
        interact.attributeDescription = attDesc;
    }

    public override void Equipped(){
        characterStats.AddStat("Vitality", 6);
    }

    public override void Unequipped(){
        characterStats.AddStat("Vitality", -6);
    }
}
