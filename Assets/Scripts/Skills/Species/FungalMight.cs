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
        
        GameObject skillLoadout = transform.parent.gameObject;
        foreach (Transform child in skillLoadout.transform)
        {
            Skill skill = child.gameObject.GetComponent<Skill>();
            if (skill != this)
            {
                skill.ActivateFungalMight(finalSkillValue);
            }
        }

        PlayerAttack playerAttack = GameObject.FindWithTag("PlayerParent").GetComponent<PlayerAttack>();
        playerAttack.ActivateFungalMight(finalSkillValue);

        FungalMightParticles();
    }

    void FungalMightParticles()
    {
        //Positions
        Vector3 rightParticlesPositionH = player.transform.position + (player.transform.right * particleDistance);
        Vector3 leftParticlesPositionH = player.transform.position + (-player.transform.right * particleDistance);
        Vector3 particlesPositionV = player.transform.up * particleHeight;
        //Rotations
        Quaternion rightParticleRotation = player.transform.rotation * Quaternion.Euler(50f, 80f, 90f);
        Quaternion leftParticleRotation = player.transform.rotation * Quaternion.Euler(50f, -80f, 0f);

        Transform[] allChildren = player.GetComponentsInChildren<Transform>();
        GameObject rightHand = null;
        GameObject leftHand = null;
        foreach(Transform child in allChildren)
        {
            if (child.gameObject.CompareTag("RightHand"))
            {
                rightHand = child.gameObject;
            }
            if (child.gameObject.CompareTag("LeftHand"))
            {
                leftHand = child.gameObject;
            }
        }

        if (rightHand != null)
        {
            ParticleManager.Instance.SpawnParticles("FungalMightParticles", rightParticlesPositionH + particlesPositionV, rightParticleRotation, rightHand);
        }
        else
        {
            Debug.LogError("Failure spawning Fungal Might particles: No child of player with tag 'RightHand'");
        }

        if (leftHand != null)
        {
            ParticleManager.Instance.SpawnParticles("FungalMightParticles", leftParticlesPositionH + particlesPositionV, leftParticleRotation, leftHand);
        }
        else 
        {
            Debug.LogError("Failure spawning Fungal Might particles: No child of player with tag 'LeftHand'");
        }

        EndSkill();
    }
}
