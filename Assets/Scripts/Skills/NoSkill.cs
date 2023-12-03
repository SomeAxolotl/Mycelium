using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoSkill : Skill
{
    public override void DoSkill()
    {
        //Skill specific stuff
        EndSkill();
    }
}
