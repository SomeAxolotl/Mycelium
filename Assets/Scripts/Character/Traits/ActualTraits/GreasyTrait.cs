using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreasyTrait : TraitBase
{
    private float dodgeCooldownReduction = 0.5f;

    public override void Start(){
        base.Start();

        traitName = "Greasy";
        traitDesc = "Dodge cooldown -" + (dodgeCooldownReduction * 100) + "%";

        controller.baseDodgeCooldown *= dodgeCooldownReduction;
    }

    void OnDestroy(){
        controller.baseDodgeCooldown /= dodgeCooldownReduction;
    }
}
