using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefenseMechanism : Skill
{
    //Skill specific fields
    private float defenseDuration = 5f;
    public override void DoSkill()
    {
        //Skill specific stuff
        StartCoroutine(DoDefense());
        EndSkill();
    }
    IEnumerator DoDefense()
    {
        playerHealth.isDefending = true;
        ParticleManager.Instance.SpawnParticles("DefenseParticles", new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), Quaternion.Euler(-90f, 0f, 0f), player);
        yield return new WaitForSeconds(defenseDuration);
        playerHealth.isDefending = false;
    }
}
