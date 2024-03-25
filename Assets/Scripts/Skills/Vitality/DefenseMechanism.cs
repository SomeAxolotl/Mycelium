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
        DefenseParticles();

        if (isPlayerCurrentPlayer())
        {
            StartCoroutine(DoDefense());
        }

        EndSkill();
    }
    void DefenseParticles()
    {
        ParticleManager.Instance.SpawnParticles("DefenseParticles", new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z), Quaternion.Euler(-90f, 0f, 0f), player);
    }

    IEnumerator DoDefense()
    {
        playerHealth.isDefending = true;
        yield return new WaitForSeconds(defenseDuration);
        playerHealth.isDefending = false;
    }
}
