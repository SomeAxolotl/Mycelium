using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FungalMight : Skill
{
    public override void DoSkill()
    {
        ClearFungalMight();

        GameObject skillLoadout = GameObject.FindWithTag("currentPlayer").transform.Find("SkillLoadout").gameObject;
        foreach (Transform child in skillLoadout.transform)
        {
            Skill skill = child.gameObject.GetComponent<Skill>();
            if (skill != this)
            {
                skill.ActivateFungalMight(finalSkillValue);
            }
        }

        //FungalMightParticles();
    }

    /*void FungalMightParticles()
    {
        GameObject player = GameObject.FindWithTag("currentPlayer");
        Vector3 leftParticlesPositionAdder = new Vector3(-0.5f, -0.5f, 0f);
        Vector3 rightParticlesPositionAdder = new Vector3(0.5f, -0.5f, 0f);

        ParticleManager.Instance.SpawnParticles("FungalMightParticles", player.transform.position + leftParticlesPositionAdder, Quaternion.identity, player);
        ParticleManager.Instance.SpawnParticles("FungalMightParticles", player.transform.position + rightParticlesPositionAdder, Quaternion.identity, player);
    }*/
}
