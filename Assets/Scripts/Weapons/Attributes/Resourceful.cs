using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resourceful : AttributeBase
{
    PlayerHealth health;

    public override void Initialize(){
        attName = "Resourceful";
        attDesc = "\nGain buffs from salvaging";
        if(stats == null || hit == null){return;}
        stats.wpnName = attName + " " + stats.wpnName;
        interact.attributeDescription = attDesc;
    }

    public override void Equipped(){
        base.Equipped();
        health = playerParent.GetComponent<PlayerHealth>();
        Actions.SalvagedWeapon += SalvageBuff;
    }

    public override void Unequipped(){
        Actions.SalvagedWeapon -= SalvageBuff;
    }

    private void SalvageBuff(GameObject weapon){
        SpeedChange speedChangeEffect = player.AddComponent<SpeedChange>();
        //For the 20 seconds, the speed will not go under 30
        speedChangeEffect.InitializeSpeedChange(30f, 50);

        if(health != null){
            health.SpawnHealingOrb(weapon.transform.position, health.maxHealth * 0.25f);
        }
    }
}
