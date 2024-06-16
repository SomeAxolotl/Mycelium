using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartyPantsTrait : TraitBase
{
    public override void Start(){
        base.Start();

        traitName = "Smarty Pants";
        traitDesc = "\nRandomly reduce cooldowns";
    }

    public void OnEnable(){
        Actions.ActivatedSkill += ReduceSkillCooldown;
    }
    public void OnDisable(){
        Actions.ActivatedSkill += ReduceSkillCooldown;
    }

    //15% chance to set skills cooldown to 25% of their cooldown
    private void ReduceSkillCooldown(Skill skill){
        if(Random.Range(0, 100) < 15){
            skill.finalSkillCooldown *= 0.25f;
        }
    }
}
