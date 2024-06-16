using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreasyTrait : TraitBase
{
    public override void Start(){
        base.Start();

        traitName = "Greasy";
        traitDesc = "\nHalves dash cooldown";

        Debug.Log(controller.baseDodgeCooldown);

        controller.baseDodgeCooldown *= 0.5f;
    }

    void OnDestroy(){
        controller.baseDodgeCooldown /= 0.5f;
    }
}
