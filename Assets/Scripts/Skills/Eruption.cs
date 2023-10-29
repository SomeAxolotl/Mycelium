using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eruption : Skill
{
    //                    ^
    //inherits the base <Skill> class. look at that class to see what fields and methods it already has

    //player controller will call ActivateSkill() on whatever class of Type<Skill> is on the corresponding SkillLoadout child
    //for example:
    // 
    // SkillLoadout
    //    Eruption
    //    Blitz
    //    Fungal Might
    //
    //pressing Stat_Skill_1 on player controller will get whatever script has the <Skill> class (which would be this script)

    //the only fields required in THIS script are whatever is unique to eruption (aoe hitboxes / other shit)
    //stuff thats serialized in the base class AND this class will both be in the inspector for this (which is why the damages and cooldowns are in inspector)

    public override void DoSkill()
    {
        //do eruption
        Debug.Log("Casting Eruption!");
        StartCooldown();
    }
}
