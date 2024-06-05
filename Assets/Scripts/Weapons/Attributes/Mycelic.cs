using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mycelic : AttributeBase
{
    private bool hitSomething = false;
    private float chanceToTrigger = 0.20f;
    GameObject skillLoadout;
    Skill skillToUse;

    public override void Initialize(){
        attName = "Mycelic";
        attDesc = "Chance to trigger species skill";
    }

    public override void Hit(GameObject target, float damage){
        hitSomething = true;
    }

    public override void Equipped()
    {
        base.Equipped();

        skillLoadout = player.transform.Find("SkillLoadout").gameObject;
        skillToUse = skillLoadout.transform.GetChild(0).gameObject.GetComponent<Skill>();
    }

    public override void StopAttack(){
        if (hitSomething)
        {
            float randomNumber = Random.Range(0f, 1f);
            if (randomNumber < chanceToTrigger)
            {
                skillToUse.ActivateSkill(0, true);
            }
        }
        hitSomething = false;
    }
}
