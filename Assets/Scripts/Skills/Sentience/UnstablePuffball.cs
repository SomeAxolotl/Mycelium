using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnstablePuffball : Skill
{
    //Skill specific fields
    /*
    damage = 20
    range = 10
    AoE radius = 2
    speed = 3
    gravity = yes

    shot projectile in an arc
    collision with enemy/terrain:
        explosion (AoE)
    travels at 3m per second for max of 10m
    explodes with a 2m radius of damage
    */

    public override void DoSkill()
    {
        //Skill specific stuff

        EndSkill();
    }
}
