using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sticky : AttributeBase
{
    public override void Initialize(){
        attName = "Sticky";
        attDesc = "Apply 50% slow for 2 seconds";
    }

    public override void Hit(GameObject target, float damage){
        SpeedChange speedChangeEffect = target.AddComponent<SpeedChange>();
        speedChangeEffect.InitializeSpeedChange(2, -50);
    }
}
