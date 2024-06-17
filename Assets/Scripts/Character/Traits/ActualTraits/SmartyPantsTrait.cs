using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmartyPantsTrait : TraitBase
{
    private float cooldownReduction = 0.25f;
    private float percentChance = 15;

    public override void Start(){
        base.Start();

        traitName = "Smarty Pants";
        traitDesc = "Cooldowns randomly reduced by " + (cooldownReduction * 100) + "%";
    }

    public override void SporeSelected(){
        Actions.ActivatedSkill += ReduceSkillCooldown;
    }
    public override void SporeUnselected(){
        Actions.ActivatedSkill += ReduceSkillCooldown;
    }

    //15% chance to set skills cooldown to 25% of their cooldown
    private void ReduceSkillCooldown(Skill skill){
        if(Random.Range(0, 100) < percentChance){
            skill.finalSkillCooldown *= cooldownReduction;
        }
    }
}
