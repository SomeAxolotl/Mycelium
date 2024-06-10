using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mycelic : AttributeBase
{
    private bool hitSomething = false;
    private float chanceToTrigger = 20f;
    GameObject skillLoadout;
    Skill skillToUse;

    WeaponSkillProc skillProc = new WeaponSkillProc();

    public override void Initialize(){
        attName = "Mycelic";
        attDesc = "Chance to trigger species skill";
    }

    public override void Equipped()
    {
        base.Equipped();

        skillLoadout = player.transform.Find("SkillLoadout").gameObject;
        skillToUse = skillLoadout.transform.GetChild(0).gameObject.GetComponent<Skill>();

        skillProc.triggerChance = chanceToTrigger;
        skillProc.skillInstance = skillToUse;

        stats.skillChances.Add(skillProc);
    }

    public override void Unequipped()
    {
        base.Unequipped();

        stats.skillChances.Remove(skillProc);
    }
}
