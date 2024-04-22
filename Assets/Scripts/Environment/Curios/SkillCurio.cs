using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillCurio : Curio
{
    public override IEnumerator DoEvent(WanderingSpore wanderingSpore)
    {
        yield return null;

        GameObject skillLoadout = wanderingSpore.transform.Find("SkillLoadout").gameObject;
        
        int randomNumber = Random.Range(0, 3);
        Skill skillToUse = skillLoadout.transform.GetChild(randomNumber).gameObject.GetComponent<Skill>();
        if (skillToUse.canSkill)
        {
            skillToUse.ActivateSkill(randomNumber);
        }
    }
}
