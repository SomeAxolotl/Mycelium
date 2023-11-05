using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FungalMight : Skill
{
    [SerializeField] private float particleDistance;
    [SerializeField] private float particleHeight;

    public override void DoSkill()
    {
        ClearAllFungalMights();
        
        GameObject skillLoadout = GameObject.FindWithTag("currentPlayer").transform.Find("SkillLoadout").gameObject;
        foreach (Transform child in skillLoadout.transform)
        {
            Skill skill = child.gameObject.GetComponent<Skill>();
            if (skill != this)
            {
                skill.ActivateFungalMight(finalSkillValue);
            }
        }

        NewPlayerAttack playerAttack = GameObject.FindWithTag("PlayerParent").GetComponent<NewPlayerAttack>();
        playerAttack.ActivateFungalMight(finalSkillValue);

        FungalMightParticles();
    }

    void FungalMightParticles()
    {
        GameObject player = GameObject.FindWithTag("currentPlayer");
        //Positions
        Vector3 rightParticlesPositionH = player.transform.position + (player.transform.right * particleDistance);
        Vector3 leftParticlesPositionH = player.transform.position + (-player.transform.right * particleDistance);
        Vector3 particlesPositionV = player.transform.up * particleHeight;
        //Rotations
        Quaternion rightParticleRotation = player.transform.rotation * Quaternion.Euler(50f, 80f, 90f);
        Quaternion leftParticleRotation = player.transform.rotation * Quaternion.Euler(50f, -80f, -00f);

        ParticleManager.Instance.SpawnParticles("FungalMightParticles", rightParticlesPositionH + particlesPositionV, rightParticleRotation, player);
        ParticleManager.Instance.SpawnParticles("FungalMightParticles", leftParticlesPositionH + particlesPositionV, leftParticleRotation, player);
    }
}
