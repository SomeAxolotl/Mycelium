using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nasty : AttributeBase
{
    public override void Initialize(){
        attName = "Nasty";
        attDesc = "20% chance to apply poison";
    }

    public override void Hit(GameObject target, float damage){
        int randomChance = Random.Range(0, 100);
        if(randomChance <= 20){
            Poison poisonEffect = target.AddComponent<Poison>();
            poisonEffect.PoisonStats(characterStats);
        }
    }
}
