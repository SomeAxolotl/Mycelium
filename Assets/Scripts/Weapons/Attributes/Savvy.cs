using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Savvy : AttributeBase
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
        attName = "Savvy";
        attDesc = "\n<sprite="+2+"> +3 <sprite="+1+"> +3";
        stats.wpnName = attName + " " + stats.wpnName;
        interact.attributeDescription = attDesc;
    }

    public override void Equipped(){
        characterStats.AddStat("Sentience", 3);
        characterStats.AddStat("Speed", 3);
    }

    public override void Unequipped(){
        characterStats.AddStat("Sentience", -3);
        characterStats.AddStat("Speed", -3);
    }
}
