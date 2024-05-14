using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smart : AttributeBase
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
        attName = "Smart";
        attDesc = "\nIncrease Sentience";
        stats.wpnName = attName + " " + stats.wpnName;
        interact.attributeDescription = attDesc;
    }

    public override void Equipped(){
        characterStats.AddStat("Sentience", 6);
    }

    public override void Unequipped(){
        characterStats.AddStat("Sentience", -6);
    }
}
