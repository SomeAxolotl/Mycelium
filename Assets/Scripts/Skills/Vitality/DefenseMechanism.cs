using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseMechanism : Skill
{
    //Skill specific fields
    [SerializeField] private float defenseDuration = 1f;
    public override void DoSkill()
    {
        //Skill specific stuff
        StartCoroutine(DoDefense());
        EndSkill();
    }
    IEnumerator DoDefense()
    {
        playerHealth.isDefending = true;
        yield return new WaitForSeconds(defenseDuration);
        playerHealth.isDefending = false;
    }
}
