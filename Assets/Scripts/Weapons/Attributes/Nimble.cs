using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nimble : AttributeBase
{
    private bool hitSomething = false;

    public override void Initialize(){
        attName = "Nimble";
        attDesc = "\nMove faster after hitting enemies";
        if(stats == null || hit == null){return;}
        stats.wpnName = attName + " " + stats.wpnName;
        interact.attributeDescription = attDesc;
    }

    public override void Hit(GameObject target, float damage){
        hitSomething = true;
    }

    public override void StopAttack(){
        if(hitSomething){
            SpeedChange speedChangeEffect = player.AddComponent<SpeedChange>();
            //90% speed boost that goes down
            speedChangeEffect.InitializeSpeedChange(1.5f, 90, true);
            //For the 1.5 seconds, the speed will not go under 30
            speedChangeEffect.InitializeSpeedChange(1.5f, 30);
        }
        hitSomething = false;
    }
}
